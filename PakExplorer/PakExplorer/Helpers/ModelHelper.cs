using PakExplorer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Helpers
{
    public static class ModelHelper
    {
        public static bool ExportMdlFrameToObj(string filename, GfxMdl model, int frameNum)
        {
            if (!string.IsNullOrWhiteSpace(filename) && model.Frames != null && model.Frames.Count > 0 && model.Frames.Count > frameNum)
            {
                if (filename.ToUpper().EndsWith(".OBJ"))
                {
                    filename = filename.Substring(0, filename.Length - 4);
                }
                var fileParts = filename.Split("\\");
                if (!File.Exists(filename + ".mtl"))
                {
                    StringBuilder mtlFile = new StringBuilder();
                    mtlFile.AppendLine("newmtl initialShadingGroup");
                    mtlFile.AppendLine("illum 4");
                    mtlFile.AppendLine("Kd 0.50 0.50 0.50");
                    mtlFile.AppendLine("Ka 0.00 0.00 0.00");
                    mtlFile.AppendLine("Tf 1.00 1.00 1.00");
                    mtlFile.AppendLine("Ni 1.00");
                    mtlFile.AppendLine("Ks 0.00 0.00 0.00");
                    mtlFile.AppendLine("Ns 18.00");
                    var mtlOut = mtlFile.ToString();
                    File.WriteAllText(filename + ".mtl", mtlOut);
                }
                StringBuilder objFile = new StringBuilder();
                objFile.AppendLine("mtllib " + fileParts[fileParts.Length - 1] + ".mtl");
                objFile.AppendLine("# Vertices");

                foreach (var vert in model.Frames[frameNum].Frame.Verts)
                {
                    var mdlVertex = vert.Vertices;
                    var vector = new Vector3();
                    vector.Z = ((mdlVertex[0] * model.Header.scale.X) + model.Header.translate.X) * 0.1f;
                    vector.X = ((mdlVertex[1] * model.Header.scale.Y) + model.Header.translate.Y) * 0.1f;
                    vector.Y = ((mdlVertex[2] * model.Header.scale.Z) + model.Header.translate.Z) * 0.1f;
                    objFile.AppendLine("v " + vector.X + " " + vector.Y + " " + vector.Z);
                }
                foreach (var vert in model.TextureCoords)
                {
                    var texCoord = new Vector2();
                    if (vert.OnSeam)
                    {
                        texCoord.X = (float)(vert.S + model.Header.skinWidth / 2.0f) / model.Header.skinWidth;
                    }
                    else
                    {
                        texCoord.X = (vert.S / (float)model.Header.skinWidth);
                    }
                    texCoord.Y = (vert.T / (float)model.Header.skinHeight);
                    objFile.AppendLine("vt " + texCoord.X + " " + texCoord.Y);
                }
                foreach (var vert in model.Frames[frameNum].Frame.Verts)
                {
                    if (vert.Normal > model.Normals.Count)
                    {
                        vert.Normal = (byte)(model.Normals.Count - 1);
                    }
                    var mdlNormal = model.Normals[vert.Normal];
                    var newNormal = new Vector3();
                    newNormal.Z = mdlNormal.X;
                    newNormal.X = mdlNormal.Y;
                    newNormal.Y = mdlNormal.Z;
                    objFile.AppendLine("vn " + newNormal.X + " " + newNormal.Y + " " + newNormal.Z);
                }
                objFile.AppendLine("#Frame");
                for (int i = 0; i < model.Triangles.Count; i++)
                {
                    StringBuilder lineBuilder = new StringBuilder("f ");
                    lineBuilder.Append((model.Triangles[i].VertexIndex[0] + 1) + "/" + (model.Triangles[i].VertexIndex[0] + 1) + "/" + (model.Triangles[i].VertexIndex[0] + 1) + " ");
                    lineBuilder.Append((model.Triangles[i].VertexIndex[1] + 1) + "/" + (model.Triangles[i].VertexIndex[1] + 1) + "/" + (model.Triangles[i].VertexIndex[1] + 1) + " ");
                    lineBuilder.Append((model.Triangles[i].VertexIndex[2] + 1) + "/" + (model.Triangles[i].VertexIndex[2] + 1) + "/" + (model.Triangles[i].VertexIndex[2] + 1));
                    objFile.AppendLine(lineBuilder.ToString());
                }
                File.WriteAllText(filename + "_" + model.Frames[frameNum].Frame.Name + "_" + frameNum + ".obj", objFile.ToString());
                return true;
            }
            return false;
        }

        public static bool ExportObjFromMdl(string filename, GfxMdl model)
        {
            if (!string.IsNullOrWhiteSpace(filename) && model.Frames != null && model.Frames.Count > 0)
            {
                if (filename.ToUpper().EndsWith(".OBJ"))
                {
                    filename = filename.Substring(0, filename.Length - 4);
                }
                bool source = true;
                for (int i = 0; i < model.Frames.Count; i++)
                {
                    source = ExportMdlFrameToObj(filename, model, i);
                    if (source == false)
                    {
                        return false;
                    }
                }
                return source;
            }
            return false;
        }

        public static bool ExportBspToObj(string filename, GfxBSPLoader bsp)
        {
            if (!string.IsNullOrWhiteSpace(filename) && bsp != null && bsp.Models.Count > 0)
            {
                string directory = Path.GetDirectoryName(filename);
                string extension = Path.GetExtension(filename);
                string shortfilename = Path.GetFileNameWithoutExtension(filename);

                string mtlFilename = Path.Combine(directory, shortfilename + ".mtl");
                string objFilename = Path.Combine(directory, shortfilename + ".obj");

                ObjModel bspObj = BSPHelper.ConvertBSPToObj(bsp);
                if (bspObj != null && bspObj.Meshes != null && bspObj.Meshes.Count > 0)
                {
                    StringBuilder objFile = new StringBuilder();
                    StringBuilder mtlFile = new StringBuilder();
                    var materials = bspObj.Meshes.SelectMany(sm => sm.Faces).Select(s => s.Texture).Distinct().ToList();
                    if (materials != null && materials.Count > 0)
                    {
                        foreach (var material in materials)
                        {
                            mtlFile.AppendLine("newmtl " + material);
                            mtlFile.AppendLine("illum 4");
                            mtlFile.AppendLine("Kd 0.50 0.50 0.50");
                            mtlFile.AppendLine("Ka 0.00 0.00 0.00");
                            mtlFile.AppendLine("Tf 1.00 1.00 1.00");
                            mtlFile.AppendLine("Ni 1.00");
                            mtlFile.AppendLine("Ks 0.00 0.00 0.00");
                            mtlFile.AppendLine("Ns 18.00");
                            mtlFile.AppendLine("");
                        }
                        var mtlOut = mtlFile.ToString();

                        File.WriteAllText(mtlFilename, mtlOut);
                    }
                    objFile.AppendLine("mtllib " + mtlFilename);
                    foreach (var mesh in bspObj.Meshes)
                    {
                        if (mesh != null)
                        {
                            for (int i = 0; i < mesh.Faces.Count; i++)
                            {
                                objFile.AppendLine("usemtl " + mesh.Faces[i].Texture);
                                objFile.AppendLine("o " + "Face" + i.ToString("000"));
                                for (int v = 0; v < mesh.Faces[i].Vertices.Count; v++)
                                {
                                    objFile.AppendLine("v " + mesh.Faces[i].Vertices[v].X + " " + mesh.Faces[i].Vertices[v].Y + " " + mesh.Faces[i].Vertices[v].Z);
                                }
                                for (int tc = 0; tc < mesh.Faces[i].TexCoords.Count; tc++)
                                {
                                    objFile.AppendLine("vt " + mesh.Faces[i].TexCoords[tc].X + " " + mesh.Faces[i].TexCoords[tc].Y);
                                }
                                for (int n = 0; n < mesh.Faces[i].Normals.Count; n++)
                                {
                                    objFile.AppendLine("vn " + mesh.Faces[i].Normals[n].X + " " + mesh.Faces[i].Normals[n].Y + " " + mesh.Faces[i].Normals[n].Z);
                                }
                                for (int vr = 0; vr < mesh.Faces[i].FaceVertices.Count; vr++)
                                {
                                    objFile.AppendLine("f " + mesh.Faces[i].FaceVertices[vr].VertexRef + "/" + mesh.Faces[i].FaceVertices[vr].TexCoordRef + "/" + mesh.Faces[i].FaceVertices[vr].NormalRef);
                                }
                            }
                        }
                    }
                    var objOut = objFile.ToString();
                    File.WriteAllText(objFilename, objOut);
                    return true;
                }
            }
            return false;
        }
    }
}
