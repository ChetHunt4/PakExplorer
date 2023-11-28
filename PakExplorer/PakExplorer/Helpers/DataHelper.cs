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
    public static class DataHelper
    {
        public static PakHeader ReadHeader(BinaryReader br)
        {
            var pakHeader = new PakHeader
            {
                magic = br.ReadChars(4)
            };
            string magic = new string(pakHeader.magic);
            if (magic != "PACK")
                throw new InvalidOperationException("Not a valid pak file");
            pakHeader.offIndex = br.ReadInt32();
            pakHeader.lenIndex = br.ReadInt32();
            return pakHeader;
        }

        public static PakFile ReadFile(BinaryReader br)
        {
            string name = new string(br.ReadChars(56));
            int index = name.IndexOf('\0');
            if (index != -1)
            {
                name = name.Substring(0, index);

                PakFile pak = new PakFile
                {
                    name = name,
                    offset = br.ReadInt32(),
                    size = br.ReadInt32()
                };

                //_pakFiles.Add(pak.name, pak);

                return pak;
            }
            throw new InvalidOperationException("Could not parse pak file!");
        }

        public static PakData? GetPakData(string filename)
        {
            if (File.Exists(filename))
            {
                PakData package = new PakData();
                package.fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                package.reader = new BinaryReader(package.fs);
                var header = ReadHeader(package.reader);
                int lenIndex = header != null ? header.lenIndex : 0;
                int size = lenIndex / 64;
                int offIndex = header != null ? header.offIndex : 0;
                package.reader.BaseStream.Position = offIndex;
                Dictionary<string, PakFile> files = new Dictionary<string, PakFile>();
                for (int i = 0; i < size; i++)
                {
                    /*var pak =*/
                    var pak = ReadFile(package.reader);
                    files.Add(pak.name, pak);

                }
                package.Header = header;
                package.PakFiles = files;
                return package;
            }
            return null;
        }

        public static byte[] ExtractFile(string pakFilename, PakData data)
        {
            byte[] buffer;
            if (data.PakFiles.ContainsKey(pakFilename))
            {
                var pak = data.PakFiles[pakFilename];
                if (pak != null)
                {
                    var stream = data.reader.BaseStream;
                    stream.Position = pak.offset;
                    buffer = data.reader.ReadBytes(pak.size);
                    return buffer;

                }

            }
            return null;
        }

        public static QuakeFileType GetFileType(string pakFilename)
        {
            if (!string.IsNullOrWhiteSpace(pakFilename))
            {
                var allParts = pakFilename.Split('/');
                if (allParts != null && allParts.Length > 0)
                {
                    var lastPart = allParts[allParts.Length - 1];
                    if (!string.IsNullOrWhiteSpace(lastPart))
                    {
                        if (lastPart.ToUpper() == "PALETTE.LMP")
                        {
                            return QuakeFileType.Palette;
                        }
                        if (lastPart.ToUpper() == "COLORMAP.LMP")
                        {
                            return QuakeFileType.Colormap;
                        }
                        else
                        {
                            var fileParts = lastPart.Split(".");
                            if (fileParts != null && fileParts.Length > 1)
                            {
                                switch (fileParts[1].ToUpper())
                                {
                                    case "LMP":
                                        return QuakeFileType.LMP;
                                    case "MDL":
                                        return QuakeFileType.Model;
                                    case "BSP":
                                        return QuakeFileType.BSP;
                                    case "WAV":
                                        return QuakeFileType.Sound;
                                    case "SPR":
                                        return QuakeFileType.Sprite;
                                    case "WAD":
                                        return QuakeFileType.Wad;
                                }
                            }
                        }

                    }
                }
            }
            return QuakeFileType.Unknown;
        }
    }

    public static class BSPHelper
    {
        public static ObjModel ConvertBSPToObj(GfxBSPLoader Bsp)
        {
            if (Bsp == null || Bsp.Models == null || Bsp.Models.Count <= 0)
            {
                return null;
            }
            var obj = new ObjModel();
            var baseModel = Bsp.Models[0];
            List<int> markedFaces = new List<int>();
            var faceIdList = Bsp.Faces.GetRange(baseModel.FaceID, baseModel.NumFaces);
            var objMesh = new ObjMesh();
            objMesh.Faces = new List<ObjFace>();
            for (int i = 0; i < faceIdList.Count; i++)
            {
                objMesh.Name = "Main";
                var l_edges = Bsp.EdgeList.GetRange(faceIdList[i].ListEdgeId, faceIdList[i].ListEdgeNum);
                var texInfo = Bsp.TextureInfo[faceIdList[i].TextureInfoId];
                var textureName = Bsp.TextureIndices[texInfo.TextureId];
                var texture = Bsp.WallTextures[textureName];

                Vector3 originVertex = Vector3.Zero;
                ObjFace face = new ObjFace();
                face.Texture = textureName;
                face.FaceVertices = new List<ObjFaceVertex>();
                face.Vertices = new List<Vector3>();
                face.TexCoords = new List<Vector2>();
                face.Normals = new List<Vector3>();
                for (int j = 0; j < l_edges.Count; j++)
                {
                    var edge = Bsp.Edges[Math.Abs(l_edges[j])];
                    var vert1 = Bsp.Vertices[edge.Vertex1];
                    var vert2 = Bsp.Vertices[edge.Vertex2];
                    if (l_edges[j] < 0)
                    {
                        //if -negative invert
                        var placeholder = vert1;
                        vert1 = vert2;
                        vert2 = placeholder;
                    }

                    if (originVertex == Vector3.Zero)
                    {
                        originVertex = vert1;
                    }
                    else
                    {
                        if (face.FaceVertices == null)
                        {
                            face.FaceVertices = new List<ObjFaceVertex>();
                        }
                        Vector3 tri1 = new Vector3(originVertex.Y * 0.1f, originVertex.Z * 0.1f, originVertex.X * 0.1f);
                        Vector3 tri2 = new Vector3(vert1.Y * 0.1f, vert1.Z * 0.1f, vert1.X * 0.1f);
                        Vector3 tri3 = new Vector3(vert2.Y * 0.1f, vert2.Z * 0.1f, vert2.X * 0.1f);
                        Vector2 tx1 = GetTexCoordFromBsp(originVertex, texInfo, texture.Width, texture.Height);
                        Vector2 tx2 = GetTexCoordFromBsp(vert1, texInfo, texture.Width, texture.Height);
                        Vector2 tx3 = GetTexCoordFromBsp(vert2, texInfo, texture.Width, texture.Height);
                        Vector3 norm1 = Vector3.Normalize(CrossProduct(tri1, tri2, tri3));
                        face.Vertices.Add(tri1);
                        face.TexCoords.Add(tx1);
                        face.Normals.Add(norm1);
                        face.FaceVertices.Add(new ObjFaceVertex
                        {
                            VertexRef = face.Vertices.Count,
                            TexCoordRef = face.TexCoords.Count,
                            NormalRef = face.Normals.Count
                        });
                        face.Vertices.Add(tri2);
                        face.TexCoords.Add(tx2);
                        face.Normals.Add(norm1);
                        face.FaceVertices.Add(new ObjFaceVertex
                        {
                            VertexRef = face.Vertices.Count,
                            TexCoordRef = face.TexCoords.Count,
                            NormalRef = face.Normals.Count
                        });
                        face.Vertices.Add(tri3);
                        face.TexCoords.Add(tx3);
                        face.Normals.Add(norm1);
                        face.FaceVertices.Add(new ObjFaceVertex
                        {
                            VertexRef = face.Vertices.Count,
                            TexCoordRef = face.TexCoords.Count,
                            NormalRef = face.Normals.Count
                        });
                    }
                }
                objMesh.Faces.Add(face);
            }
            if (obj.Meshes == null)
            {
                obj.Meshes = new List<ObjMesh>();
            }
            obj.Meshes.Add(objMesh);
            return obj;
        }

        private static Vector2 GetTexCoordFromBsp(Vector3 worldVector, BSPTextureInfo texInfo, int width, int height)
        {
            var s = (Vector3.Dot(worldVector, texInfo.VectorS) + texInfo.DistS) / width;
            var t = (Vector3.Dot(worldVector, texInfo.VectorT) + texInfo.DistT) / height;
            return new Vector2(s, t);
        }

        private static Vector3 CrossProduct(Vector3 a, Vector3 b, Vector3 c)
        {
            // Calculate the vectors representing the sides of the parallelogram
            Vector3 side1 = new Vector3(b.X - a.X, b.Y - a.Y, b.Z - a.Z);
            Vector3 side2 = new Vector3(c.X - a.X, c.Y - a.Y, c.Z - a.Z);

            // Calculate the cross product of the two sides
            float crossX = (side1.Y * side2.Z) - (side1.Z * side2.Y);
            float crossY = (side1.Z * side2.X) - (side1.X * side2.Z);
            float crossZ = (side1.X * side2.Y) - (side1.Y * side2.X);

            // Return the resulting vector
            return new Vector3(crossX, crossY, crossZ);
        }
    }
}
