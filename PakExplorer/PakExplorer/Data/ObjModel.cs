using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public class ObjFace
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector2> TexCoords { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<ObjFaceVertex> FaceVertices { get; set; }
        public string Texture { get; set; }
    }
    public class ObjFaceVertex
    {
        public int VertexRef { get; set; }
        public int NormalRef { get; set; }
        public int TexCoordRef { get; set; }
    }

    public class ObjMesh
    {
        public string Name { get; set; }
        public List<ObjFace> Faces { get; set; }
        public List<Vector3> Vertices { get; set; }
        public List<Vector2> TextureCoords { get; set; }
        public List<Vector3> Normals { get; set; }
    }

    public class ObjModel
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> VertexNormals { get; set; }
        public List<Vector2> TextureCoords { get; set; }
        public List<ObjMesh> Meshes { get; set; }
        public string Filename { get; set; }

    }
}
