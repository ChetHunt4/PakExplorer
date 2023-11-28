using PakExplorer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public class MdlHeader
    {
        public char[] magic { get; set; }
        public int version { get; set; }
        public Vector3 scale { get; set; }
        public Vector3 translate { get; set; }
        public float boundingRadius { get; set; }
        public Vector3 eyepos { get; set; }

        public int numSkins { get; set; }
        public int skinWidth { get; set; }
        public int skinHeight { get; set; }

        public int numVerts { get; set; }
        public int numTris { get; set; }
        public int numFrames { get; set; }

        public int syncType { get; set; }
        public int flags { get; set; }
        public float size { get; set; }

    }

    public class MD2Header
    {
        public char[] ident { get; set; }                  /* magic number: "IDP2" */
        public int version { get; set; }                /* version: must be 8 */

        public int skinwidth { get; set; }              /* texture width */
        public int skinheight { get; set; }             /* texture height */

        public int framesize { get; set; }              /* size in bytes of a frame */

        public int num_skins { get; set; }              /* number of skins */
        public int num_vertices { get; set; }           /* number of vertices per frame */
        public int num_st { get; set; }                 /* number of texture coordinates */
        public int num_tris { get; set; }               /* number of triangles */
        public int num_glcmds { get; set; }             /* number of opengl commands */
        public int num_frames { get; set; }             /* number of frames */

        public int offset_skins { get; set; }           /* offset skin data */
        public int offset_st { get; set; }              /* offset texture coordinate data */
        public int offset_tris { get; set; }            /* offset triangle data */
        public int offset_frames { get; set; }          /* offset frame data */
        public int offset_glcmds { get; set; }          /* offset OpenGL command data */
        public int offset_end { get; set; }             /* offset end of file */
    }

    public class QTextureCoord
    {
        public bool OnSeam { get; set; }
        public int S { get; set; }
        public int T { get; set; }
    }

    public class Q2TextureCoord
    {
        public int S { get; set; }
        public int T { get; set; }
    }

    public class QTriangle
    {
        public bool FrontFacing { get; set; }
        public List<int> VertexIndex { get; set; }
    }

    public class Q2Triangle
    {
        public int[] VertexIndices { get; set; }
        public int[] TextureIndices { get; set; }
    }

    public class QVertex
    {
        public List<byte> Vertices { get; set; }
        public byte Normal { get; set; }

    }

    public class QMdlFrameSimple
    {
        public QVertex BBoxMin { get; set; }
        public QVertex BBoxMax { get; set; }
        public string Name { get; set; }
        public List<QVertex> Verts { get; set; }
    }

    public class QMdlFrame
    {
        public int Type { get; set; }
        public QMdlFrameSimple Frame { get; set; }
    }

    public class Q2Md2Frame
    {
        public Vector3 Scale { get; set; }
        public Vector3 Position { get; set; }
        public string Name { get; set; }
        public List<QVertex> Verts { get; set; }
    }

    public class QMdlGroupFrame : QMdlFrame
    {
        public QVertex MinFrame { get; set; }
        public QVertex MaxFrame { get; set; }
        public float Duration { get; set; }
    }

    public class QGLCommand
    {
        public float s { get; set; }
        public float t { get; set; }
        public int index { get; set; }
    }

    public class GfxMd2
    {
        public List<Vector3> Normals { get; set; }
        //public List<GfxImg> Textures { get; set; }
        public List<string> Textures { get; set; }
        public List<Q2TextureCoord> TextureCoords { get; set; }
        public List<Q2Triangle> Triangles { get; set; }
        public List<Q2Md2Frame> Frames { get; set; }
        public List<int> GLCommands { get; set; }
        public MD2Header Header { get; set; }
        public string Name { get; set; }

        public GfxMd2(byte[] md2, BinaryReader br)
        {
            Normals = new List<Vector3> {
                    new Vector3(-0.525731f,  0.000000f,  0.850651f),
                    new Vector3(-0.442863f,  0.238856f,  0.864188f),
                    new Vector3(-0.295242f,  0.000000f,  0.955423f),
                    new Vector3(-0.309017f,  0.500000f,  0.809017f),
                    new Vector3(-0.162460f,  0.262866f,  0.951056f),
                    new Vector3(0.000000f,  0.000000f,  1.000000f),
                    new Vector3( 0.000000f,  0.850651f,  0.525731f),
                    new Vector3(-0.147621f,  0.716567f,  0.681718f),
                    new Vector3(0.147621f,  0.716567f,  0.681718f),
                    new Vector3(0.000000f,  0.525731f,  0.850651f),
                    new Vector3(0.309017f,  0.500000f,  0.809017f),
                    new Vector3(0.525731f,  0.000000f,  0.850651f),
                    new Vector3(0.295242f,  0.000000f,  0.955423f),
                    new Vector3(0.442863f,  0.238856f,  0.864188f),
                    new Vector3(0.162460f,  0.262866f,  0.951056f),
                    new Vector3(-0.681718f,  0.147621f,  0.716567f),
                    new Vector3(-0.809017f,  0.309017f,  0.500000f),
                    new Vector3(-0.587785f,  0.425325f,  0.688191f),
                    new Vector3(-0.850651f,  0.525731f,  0.000000f),
                    new Vector3(-0.864188f,  0.442863f,  0.238856f),
                    new Vector3(-0.716567f,  0.681718f,  0.147621f),
                    new Vector3(-0.688191f,  0.587785f,  0.425325f),
                    new Vector3(-0.500000f,  0.809017f,  0.309017f),
                    new Vector3(-0.238856f,  0.864188f,  0.442863f),
                    new Vector3(-0.425325f,  0.688191f,  0.587785f),
                    new Vector3(-0.716567f,  0.681718f, -0.147621f),
                    new Vector3(-0.500000f,  0.809017f, -0.309017f),
                    new Vector3(-0.525731f,  0.850651f,  0.000000f),
                    new Vector3(0.000000f,  0.850651f, -0.525731f),
                    new Vector3(-0.238856f,  0.864188f, -0.442863f),
                    new Vector3( 0.000000f,  0.955423f, -0.295242f),
                    new Vector3(-0.262866f,  0.951056f, -0.162460f),
                    new Vector3(0.000000f,  1.000000f,  0.000000f),
                    new Vector3(0.000000f,  0.955423f,  0.295242f),
                    new Vector3(-0.262866f,  0.951056f,  0.162460f),
                    new Vector3(0.238856f,  0.864188f,  0.442863f),
                    new Vector3(0.262866f,  0.951056f,  0.162460f),
                    new Vector3(0.500000f,  0.809017f,  0.309017f),
                    new Vector3(0.238856f,  0.864188f, -0.442863f),
                    new Vector3(0.262866f,  0.951056f, -0.162460f),
                    new Vector3(0.500000f,  0.809017f, -0.309017f),
                    new Vector3(0.850651f,  0.525731f,  0.000000f),
                    new Vector3(0.716567f,  0.681718f,  0.147621f),
                    new Vector3(0.716567f,  0.681718f, -0.147621f),
                    new Vector3(0.525731f,  0.850651f,  0.000000f),
                    new Vector3(0.425325f,  0.688191f,  0.587785f),
                    new Vector3(0.864188f,  0.442863f,  0.238856f),
                    new Vector3(0.688191f,  0.587785f,  0.425325f),
                    new Vector3(0.809017f,  0.309017f,  0.500000f),
                    new Vector3( 0.681718f,  0.147621f,  0.716567f),
                    new Vector3( 0.587785f,  0.425325f,  0.688191f),
                    new Vector3( 0.955423f,  0.295242f,  0.000000f),
                    new Vector3( 1.000000f,  0.000000f,  0.000000f),
                    new Vector3( 0.951056f,  0.162460f,  0.262866f),
                    new Vector3( 0.850651f, -0.525731f,  0.000000f),
                    new Vector3( 0.955423f, -0.295242f,  0.000000f),
                    new Vector3( 0.864188f, -0.442863f,  0.238856f),
                    new Vector3( 0.951056f, -0.162460f,  0.262866f),
                    new Vector3( 0.809017f, -0.309017f,  0.500000f),
                    new Vector3( 0.681718f, -0.147621f,  0.716567f ),
                    new Vector3( 0.850651f,  0.000000f,  0.525731f ),
                    new Vector3( 0.864188f,  0.442863f, -0.238856f ),
                    new Vector3( 0.809017f,  0.309017f, -0.500000f ),
                    new Vector3( 0.951056f,  0.162460f, -0.262866f ),
                    new Vector3( 0.525731f,  0.000000f, -0.850651f ),
                    new Vector3( 0.681718f,  0.147621f, -0.716567f ),
                    new Vector3( 0.681718f, -0.147621f, -0.716567f ),
                    new Vector3( 0.850651f,  0.000000f, -0.525731f ),
                    new Vector3( 0.809017f, -0.309017f, -0.500000f ),
                    new Vector3( 0.864188f, -0.442863f, -0.238856f ),
                    new Vector3( 0.951056f, -0.162460f, -0.262866f ),
                    new Vector3( 0.147621f,  0.716567f, -0.681718f ),
                    new Vector3( 0.309017f,  0.500000f, -0.809017f ),
                    new Vector3( 0.425325f,  0.688191f, -0.587785f ),
                    new Vector3( 0.442863f,  0.238856f, -0.864188f ),
                    new Vector3( 0.587785f,  0.425325f, -0.688191f ),
                    new Vector3( 0.688191f,  0.587785f, -0.425325f ),
                    new Vector3( -0.147621f,  0.716567f, -0.681718f ),
                    new Vector3( -0.309017f,  0.500000f, -0.809017f ),
                    new Vector3( 0.000000f,  0.525731f, -0.850651f ),
                    new Vector3( -0.525731f,  0.000000f, -0.850651f ),
                    new Vector3( -0.442863f,  0.238856f, -0.864188f ),
                    new Vector3( -0.295242f,  0.000000f, -0.955423f ),
                    new Vector3( -0.162460f,  0.262866f, -0.951056f ),
                    new Vector3( 0.000000f,  0.000000f, -1.000000f ),
                    new Vector3( 0.295242f,  0.000000f, -0.955423f ),
                    new Vector3( 0.162460f,  0.262866f, -0.951056f ),
                    new Vector3( -0.442863f, -0.238856f, -0.864188f ),
                    new Vector3( -0.309017f, -0.500000f, -0.809017f ),
                    new Vector3( -0.162460f, -0.262866f, -0.951056f ),
                    new Vector3( 0.000000f, -0.850651f, -0.525731f ),
                    new Vector3( -0.147621f, -0.716567f, -0.681718f ),
                    new Vector3( 0.147621f, -0.716567f, -0.681718f ),
                    new Vector3( 0.000000f, -0.525731f, -0.850651f ),
                    new Vector3( 0.309017f, -0.500000f, -0.809017f ),
                    new Vector3( 0.442863f, -0.238856f, -0.864188f ),
                    new Vector3( 0.162460f, -0.262866f, -0.951056f ),
                    new Vector3( 0.238856f, -0.864188f, -0.442863f ),
                    new Vector3( 0.500000f, -0.809017f, -0.309017f ),
                    new Vector3( 0.425325f, -0.688191f, -0.587785f ),
                    new Vector3( 0.716567f, -0.681718f, -0.147621f ),
                    new Vector3( 0.688191f, -0.587785f, -0.425325f ),
                    new Vector3( 0.587785f, -0.425325f, -0.688191f ),
                    new Vector3( 0.000000f, -0.955423f, -0.295242f ),
                    new Vector3( 0.000000f, -1.000000f,  0.000000f ),
                    new Vector3( 0.262866f, -0.951056f, -0.162460f ),
                    new Vector3( 0.000000f, -0.850651f,  0.525731f ),
                    new Vector3( 0.000000f, -0.955423f,  0.295242f ),
                    new Vector3( 0.238856f, -0.864188f,  0.442863f ),
                    new Vector3( 0.262866f, -0.951056f,  0.162460f ),
                    new Vector3( 0.500000f, -0.809017f,  0.309017f ),
                    new Vector3( 0.716567f, -0.681718f,  0.147621f ),
                    new Vector3( 0.525731f, -0.850651f,  0.000000f ),
                    new Vector3( -0.238856f, -0.864188f, -0.442863f ),
                    new Vector3( -0.500000f, -0.809017f, -0.309017f ),
                    new Vector3( -0.262866f, -0.951056f, -0.162460f ),
                    new Vector3( -0.850651f, -0.525731f,  0.000000f ),
                    new Vector3( -0.716567f, -0.681718f, -0.147621f ),
                    new Vector3( -0.716567f, -0.681718f,  0.147621f ),
                    new Vector3( -0.525731f, -0.850651f,  0.000000f ),
                    new Vector3( -0.500000f, -0.809017f,  0.309017f ),
                    new Vector3( -0.238856f, -0.864188f,  0.442863f ),
                    new Vector3( -0.262866f, -0.951056f,  0.162460f ),
                    new Vector3( -0.864188f, -0.442863f,  0.238856f ),
                    new Vector3( -0.809017f, -0.309017f,  0.500000f ),
                    new Vector3( -0.688191f, -0.587785f,  0.425325f ),
                    new Vector3( -0.681718f, -0.147621f,  0.716567f ),
                    new Vector3( -0.442863f, -0.238856f,  0.864188f ),
                    new Vector3( -0.587785f, -0.425325f,  0.688191f ),
                    new Vector3( -0.309017f, -0.500000f,  0.809017f ),
                    new Vector3( -0.147621f, -0.716567f,  0.681718f ),
                    new Vector3( -0.425325f, -0.688191f,  0.587785f ),
                    new Vector3( -0.162460f, -0.262866f,  0.951056f ),
                    new Vector3( 0.442863f, -0.238856f,  0.864188f ),
                    new Vector3( 0.162460f, -0.262866f,  0.951056f ),
                    new Vector3( 0.309017f, -0.500000f,  0.809017f ),
                    new Vector3( 0.147621f, -0.716567f,  0.681718f ),
                    new Vector3( 0.000000f, -0.525731f,  0.850651f ),
                    new Vector3( 0.425325f, -0.688191f,  0.587785f ),
                    new Vector3( 0.587785f, -0.425325f,  0.688191f ),
                    new Vector3( 0.688191f, -0.587785f,  0.425325f ),
                    new Vector3( -0.955423f,  0.295242f,  0.000000f ),
                    new Vector3( -0.951056f,  0.162460f,  0.262866f ),
                    new Vector3( -1.000000f,  0.000000f,  0.000000f ),
                    new Vector3( -0.850651f,  0.000000f,  0.525731f ),
                    new Vector3( -0.955423f, -0.295242f,  0.000000f ),
                    new Vector3( -0.951056f, -0.162460f,  0.262866f ),
                    new Vector3( -0.864188f,  0.442863f, -0.238856f ),
                    new Vector3( -0.951056f,  0.162460f, -0.262866f ),
                    new Vector3( -0.809017f,  0.309017f, -0.500000f ),
                    new Vector3( -0.864188f, -0.442863f, -0.238856f ),
                    new Vector3( -0.951056f, -0.162460f, -0.262866f ),
                    new Vector3( -0.809017f, -0.309017f, -0.500000f ),
                    new Vector3( -0.681718f,  0.147621f, -0.716567f ),
                    new Vector3( -0.681718f, -0.147621f, -0.716567f ),
                    new Vector3( -0.850651f,  0.000000f, -0.525731f ),
                    new Vector3( -0.688191f,  0.587785f, -0.425325f ),
                    new Vector3( -0.587785f,  0.425325f, -0.688191f ),
                    new Vector3( -0.425325f,  0.688191f, -0.587785f ),
                    new Vector3( -0.425325f, -0.688191f, -0.587785f ),
                    new Vector3( -0.587785f, -0.425325f, -0.688191f ),
                    new Vector3( -0.688191f, -0.587785f, -0.425325f )
            };
            //var stream = br.BaseStream;
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            //stream.Position = stream.Position - md2.Length;
            //stream.Position = 0;
            ReadHeader(md2, br);
            //stream.Position = Header.offset_skins;
            br.BaseStream.Seek(Header.offset_skins, SeekOrigin.Begin);
            ReadTextures(md2, br);
            //stream.Position = Header.offset_st;
            br.BaseStream.Seek(Header.offset_st, SeekOrigin.Begin);
            ReadTextureCoords(md2, br);
            //stream.Position = Header.offset_tris;
            br.BaseStream.Seek(Header.offset_tris, SeekOrigin.Begin);
            ReadTriangles(md2, br);
            //stream.Position = Header.offset_frames;
            br.BaseStream.Seek(Header.offset_frames, SeekOrigin.Begin);
            ReadFrames(md2, br);
            //stream.Position = Header.offset_glcmds;
            br.BaseStream.Seek(Header.offset_glcmds, SeekOrigin.Begin);
            ReadGLInstructions(md2, br);
        }

        private void ReadHeader(byte[] mdl, BinaryReader br)
        {

            Header = new MD2Header
            {
                ident = br.ReadChars(4)
            };
            string magic = new string(Header.ident);
            if (magic != "IDP2")
            {
                return;
            }
            Header.version = br.ReadInt32();
            Header.skinwidth = br.ReadInt32();
            Header.skinheight = br.ReadInt32();
            Header.framesize = br.ReadInt32();
            Header.num_skins = br.ReadInt32();
            Header.num_vertices = br.ReadInt32();
            Header.num_st = br.ReadInt32();
            Header.num_tris = br.ReadInt32();
            Header.num_glcmds = br.ReadInt32();
            Header.num_frames = br.ReadInt32();
            Header.offset_skins = br.ReadInt32();
            Header.offset_st = br.ReadInt32();
            Header.offset_tris = br.ReadInt32();
            Header.offset_frames = br.ReadInt32();
            Header.offset_glcmds = br.ReadInt32();
            Header.offset_end = br.ReadInt32();

        }

        private void ReadTextures(byte[] mdl, BinaryReader br)
        {
            Textures = new List<String>();
            for (int i = 0; i < Header.num_skins; i++)
            {
                char[] texName = br.ReadChars(64);
                var texString = new string(texName);
                var index = texString.IndexOf('\0');
                if (index >= 0)
                {
                    texString = texString.Substring(0, index);
                }
                Textures.Add(texString);
            }
        }

        private void ReadTextureCoords(byte[] mdl, BinaryReader br)
        {
            TextureCoords = new List<Q2TextureCoord>();
            for (int i = 0; i < Header.num_st; i++)
            {
                Q2TextureCoord texCoord = new Q2TextureCoord()
                {
                    //OnSeam = br.ReadInt32() == 32 ? true : false,
                    S = br.ReadInt16(),
                    T = br.ReadInt16()
                };
                TextureCoords.Add(texCoord);
            }
        }

        private void ReadTriangles(byte[] mdl, BinaryReader br)
        {
            Triangles = new List<Q2Triangle>();
            for (int i = 0; i < Header.num_tris; i++)
            {
                Q2Triangle tri = new Q2Triangle();
                //tri.FrontFacing = br.ReadInt32() == 1 ? true : false;
                //tri.VertexIndices = new List<int>();
                var vertIndices = new List<int>();
                var texIndices = new List<int>();
                for (int v = 0; v < 3; v++)
                {
                    //tri.VertexIndex.Add(br.ReadInt32());
                    vertIndices.Add(br.ReadUInt16());
                }
                for (int t = 0; t < 3; t++)
                {
                    texIndices.Add(br.ReadUInt16());

                }
                tri.VertexIndices = vertIndices.ToArray();
                tri.TextureIndices = texIndices.ToArray();
                Triangles.Add(tri);
            }
        }

        private void ReadFrames(byte[] mdl, BinaryReader br)
        {
            Frames = new List<Q2Md2Frame>();
            for (int i = 0; i < Header.num_frames; i++)
            {
                //int type = br.ReadInt32();
                Q2Md2Frame newFrame = new Q2Md2Frame();

                newFrame.Scale = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                newFrame.Position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

                char[] frameName = br.ReadChars(16);
                newFrame.Name = new string(frameName);

                int index = newFrame.Name.IndexOf('\0');

                //if progs.dat don't worry about it
                if (index >= 0)
                {
                    newFrame.Name = newFrame.Name.Substring(0, index);
                }
                newFrame.Verts = new List<QVertex>();
                for (int v = 0; v < Header.num_vertices; v++)
                {
                    QVertex vert = new QVertex
                    {
                        Vertices = new List<byte>() {
                            br.ReadByte(),
                            br.ReadByte(),
                            br.ReadByte()
                        },
                        Normal = br.ReadByte()
                    };
                    newFrame.Verts.Add(vert);
                }
                Frames.Add(newFrame);
            }
        }

        private void ReadGLInstructions(byte[] mdl, BinaryReader br)
        {
            GLCommands = new List<int>();
            for (int i = 0; i < Header.num_glcmds; i++)
            {
                //QGLCommand cmd = new QGLCommand
                //{
                //    s = br.ReadSingle(),
                //    t = br.ReadSingle(),
                //    index = br.ReadInt32()
                //};
                //GLCommands.Add(cmd);
                GLCommands.Add(br.ReadInt32());
            }
        }

    }

    public class GfxMdl
    {
        public List<Vector3> Normals { get; set; }
        public List<BitmapDataPackage> Textures { get; set; }
        public List<QTextureCoord> TextureCoords { get; set; }
        public List<QTriangle> Triangles { get; set; }
        //public List<QVertex> Vertices { get; set; }
        public List<QMdlFrame> Frames { get; set; }
        public MdlHeader Header { get; set; }

        public string Name { get; set; }


        public GfxMdl(byte[] mdl, PaletteLmp palette, BinaryReader br)
        {
            Normals = new List<Vector3> {
                    new Vector3(-0.525731f,  0.000000f,  0.850651f),
                    new Vector3(-0.442863f,  0.238856f,  0.864188f),
                    new Vector3(-0.295242f,  0.000000f,  0.955423f),
                    new Vector3(-0.309017f,  0.500000f,  0.809017f),
                    new Vector3(-0.162460f,  0.262866f,  0.951056f),
                    new Vector3(0.000000f,  0.000000f,  1.000000f),
                    new Vector3( 0.000000f,  0.850651f,  0.525731f),
                    new Vector3(-0.147621f,  0.716567f,  0.681718f),
                    new Vector3(0.147621f,  0.716567f,  0.681718f),
                    new Vector3(0.000000f,  0.525731f,  0.850651f),
                    new Vector3(0.309017f,  0.500000f,  0.809017f),
                    new Vector3(0.525731f,  0.000000f,  0.850651f),
                    new Vector3(0.295242f,  0.000000f,  0.955423f),
                    new Vector3(0.442863f,  0.238856f,  0.864188f),
                    new Vector3(0.162460f,  0.262866f,  0.951056f),
                    new Vector3(-0.681718f,  0.147621f,  0.716567f),
                    new Vector3(-0.809017f,  0.309017f,  0.500000f),
                    new Vector3(-0.587785f,  0.425325f,  0.688191f),
                    new Vector3(-0.850651f,  0.525731f,  0.000000f),
                    new Vector3(-0.864188f,  0.442863f,  0.238856f),
                    new Vector3(-0.716567f,  0.681718f,  0.147621f),
                    new Vector3(-0.688191f,  0.587785f,  0.425325f),
                    new Vector3(-0.500000f,  0.809017f,  0.309017f),
                    new Vector3(-0.238856f,  0.864188f,  0.442863f),
                    new Vector3(-0.425325f,  0.688191f,  0.587785f),
                    new Vector3(-0.716567f,  0.681718f, -0.147621f),
                    new Vector3(-0.500000f,  0.809017f, -0.309017f),
                    new Vector3(-0.525731f,  0.850651f,  0.000000f),
                    new Vector3(0.000000f,  0.850651f, -0.525731f),
                    new Vector3(-0.238856f,  0.864188f, -0.442863f),
                    new Vector3( 0.000000f,  0.955423f, -0.295242f),
                    new Vector3(-0.262866f,  0.951056f, -0.162460f),
                    new Vector3(0.000000f,  1.000000f,  0.000000f),
                    new Vector3(0.000000f,  0.955423f,  0.295242f),
                    new Vector3(-0.262866f,  0.951056f,  0.162460f),
                    new Vector3(0.238856f,  0.864188f,  0.442863f),
                    new Vector3(0.262866f,  0.951056f,  0.162460f),
                    new Vector3(0.500000f,  0.809017f,  0.309017f),
                    new Vector3(0.238856f,  0.864188f, -0.442863f),
                    new Vector3(0.262866f,  0.951056f, -0.162460f),
                    new Vector3(0.500000f,  0.809017f, -0.309017f),
                    new Vector3(0.850651f,  0.525731f,  0.000000f),
                    new Vector3(0.716567f,  0.681718f,  0.147621f),
                    new Vector3(0.716567f,  0.681718f, -0.147621f),
                    new Vector3(0.525731f,  0.850651f,  0.000000f),
                    new Vector3(0.425325f,  0.688191f,  0.587785f),
                    new Vector3(0.864188f,  0.442863f,  0.238856f),
                    new Vector3(0.688191f,  0.587785f,  0.425325f),
                    new Vector3(0.809017f,  0.309017f,  0.500000f),
                    new Vector3( 0.681718f,  0.147621f,  0.716567f),
                    new Vector3( 0.587785f,  0.425325f,  0.688191f),
                    new Vector3( 0.955423f,  0.295242f,  0.000000f),
                    new Vector3( 1.000000f,  0.000000f,  0.000000f),
                    new Vector3( 0.951056f,  0.162460f,  0.262866f),
                    new Vector3( 0.850651f, -0.525731f,  0.000000f),
                    new Vector3( 0.955423f, -0.295242f,  0.000000f),
                    new Vector3( 0.864188f, -0.442863f,  0.238856f),
                    new Vector3( 0.951056f, -0.162460f,  0.262866f),
                    new Vector3( 0.809017f, -0.309017f,  0.500000f),
                    new Vector3( 0.681718f, -0.147621f,  0.716567f ),
                    new Vector3( 0.850651f,  0.000000f,  0.525731f ),
                    new Vector3( 0.864188f,  0.442863f, -0.238856f ),
                    new Vector3( 0.809017f,  0.309017f, -0.500000f ),
                    new Vector3( 0.951056f,  0.162460f, -0.262866f ),
                    new Vector3( 0.525731f,  0.000000f, -0.850651f ),
                    new Vector3( 0.681718f,  0.147621f, -0.716567f ),
                    new Vector3( 0.681718f, -0.147621f, -0.716567f ),
                    new Vector3( 0.850651f,  0.000000f, -0.525731f ),
                    new Vector3( 0.809017f, -0.309017f, -0.500000f ),
                    new Vector3( 0.864188f, -0.442863f, -0.238856f ),
                    new Vector3( 0.951056f, -0.162460f, -0.262866f ),
                    new Vector3( 0.147621f,  0.716567f, -0.681718f ),
                    new Vector3( 0.309017f,  0.500000f, -0.809017f ),
                    new Vector3( 0.425325f,  0.688191f, -0.587785f ),
                    new Vector3( 0.442863f,  0.238856f, -0.864188f ),
                    new Vector3( 0.587785f,  0.425325f, -0.688191f ),
                    new Vector3( 0.688191f,  0.587785f, -0.425325f ),
                    new Vector3( -0.147621f,  0.716567f, -0.681718f ),
                    new Vector3( -0.309017f,  0.500000f, -0.809017f ),
                    new Vector3( 0.000000f,  0.525731f, -0.850651f ),
                    new Vector3( -0.525731f,  0.000000f, -0.850651f ),
                    new Vector3( -0.442863f,  0.238856f, -0.864188f ),
                    new Vector3( -0.295242f,  0.000000f, -0.955423f ),
                    new Vector3( -0.162460f,  0.262866f, -0.951056f ),
                    new Vector3( 0.000000f,  0.000000f, -1.000000f ),
                    new Vector3( 0.295242f,  0.000000f, -0.955423f ),
                    new Vector3( 0.162460f,  0.262866f, -0.951056f ),
                    new Vector3( -0.442863f, -0.238856f, -0.864188f ),
                    new Vector3( -0.309017f, -0.500000f, -0.809017f ),
                    new Vector3( -0.162460f, -0.262866f, -0.951056f ),
                    new Vector3( 0.000000f, -0.850651f, -0.525731f ),
                    new Vector3( -0.147621f, -0.716567f, -0.681718f ),
                    new Vector3( 0.147621f, -0.716567f, -0.681718f ),
                    new Vector3( 0.000000f, -0.525731f, -0.850651f ),
                    new Vector3( 0.309017f, -0.500000f, -0.809017f ),
                    new Vector3( 0.442863f, -0.238856f, -0.864188f ),
                    new Vector3( 0.162460f, -0.262866f, -0.951056f ),
                    new Vector3( 0.238856f, -0.864188f, -0.442863f ),
                    new Vector3( 0.500000f, -0.809017f, -0.309017f ),
                    new Vector3( 0.425325f, -0.688191f, -0.587785f ),
                    new Vector3( 0.716567f, -0.681718f, -0.147621f ),
                    new Vector3( 0.688191f, -0.587785f, -0.425325f ),
                    new Vector3( 0.587785f, -0.425325f, -0.688191f ),
                    new Vector3( 0.000000f, -0.955423f, -0.295242f ),
                    new Vector3( 0.000000f, -1.000000f,  0.000000f ),
                    new Vector3( 0.262866f, -0.951056f, -0.162460f ),
                    new Vector3( 0.000000f, -0.850651f,  0.525731f ),
                    new Vector3( 0.000000f, -0.955423f,  0.295242f ),
                    new Vector3( 0.238856f, -0.864188f,  0.442863f ),
                    new Vector3( 0.262866f, -0.951056f,  0.162460f ),
                    new Vector3( 0.500000f, -0.809017f,  0.309017f ),
                    new Vector3( 0.716567f, -0.681718f,  0.147621f ),
                    new Vector3( 0.525731f, -0.850651f,  0.000000f ),
                    new Vector3( -0.238856f, -0.864188f, -0.442863f ),
                    new Vector3( -0.500000f, -0.809017f, -0.309017f ),
                    new Vector3( -0.262866f, -0.951056f, -0.162460f ),
                    new Vector3( -0.850651f, -0.525731f,  0.000000f ),
                    new Vector3( -0.716567f, -0.681718f, -0.147621f ),
                    new Vector3( -0.716567f, -0.681718f,  0.147621f ),
                    new Vector3( -0.525731f, -0.850651f,  0.000000f ),
                    new Vector3( -0.500000f, -0.809017f,  0.309017f ),
                    new Vector3( -0.238856f, -0.864188f,  0.442863f ),
                    new Vector3( -0.262866f, -0.951056f,  0.162460f ),
                    new Vector3( -0.864188f, -0.442863f,  0.238856f ),
                    new Vector3( -0.809017f, -0.309017f,  0.500000f ),
                    new Vector3( -0.688191f, -0.587785f,  0.425325f ),
                    new Vector3( -0.681718f, -0.147621f,  0.716567f ),
                    new Vector3( -0.442863f, -0.238856f,  0.864188f ),
                    new Vector3( -0.587785f, -0.425325f,  0.688191f ),
                    new Vector3( -0.309017f, -0.500000f,  0.809017f ),
                    new Vector3( -0.147621f, -0.716567f,  0.681718f ),
                    new Vector3( -0.425325f, -0.688191f,  0.587785f ),
                    new Vector3( -0.162460f, -0.262866f,  0.951056f ),
                    new Vector3( 0.442863f, -0.238856f,  0.864188f ),
                    new Vector3( 0.162460f, -0.262866f,  0.951056f ),
                    new Vector3( 0.309017f, -0.500000f,  0.809017f ),
                    new Vector3( 0.147621f, -0.716567f,  0.681718f ),
                    new Vector3( 0.000000f, -0.525731f,  0.850651f ),
                    new Vector3( 0.425325f, -0.688191f,  0.587785f ),
                    new Vector3( 0.587785f, -0.425325f,  0.688191f ),
                    new Vector3( 0.688191f, -0.587785f,  0.425325f ),
                    new Vector3( -0.955423f,  0.295242f,  0.000000f ),
                    new Vector3( -0.951056f,  0.162460f,  0.262866f ),
                    new Vector3( -1.000000f,  0.000000f,  0.000000f ),
                    new Vector3( -0.850651f,  0.000000f,  0.525731f ),
                    new Vector3( -0.955423f, -0.295242f,  0.000000f ),
                    new Vector3( -0.951056f, -0.162460f,  0.262866f ),
                    new Vector3( -0.864188f,  0.442863f, -0.238856f ),
                    new Vector3( -0.951056f,  0.162460f, -0.262866f ),
                    new Vector3( -0.809017f,  0.309017f, -0.500000f ),
                    new Vector3( -0.864188f, -0.442863f, -0.238856f ),
                    new Vector3( -0.951056f, -0.162460f, -0.262866f ),
                    new Vector3( -0.809017f, -0.309017f, -0.500000f ),
                    new Vector3( -0.681718f,  0.147621f, -0.716567f ),
                    new Vector3( -0.681718f, -0.147621f, -0.716567f ),
                    new Vector3( -0.850651f,  0.000000f, -0.525731f ),
                    new Vector3( -0.688191f,  0.587785f, -0.425325f ),
                    new Vector3( -0.587785f,  0.425325f, -0.688191f ),
                    new Vector3( -0.425325f,  0.688191f, -0.587785f ),
                    new Vector3( -0.425325f, -0.688191f, -0.587785f ),
                    new Vector3( -0.587785f, -0.425325f, -0.688191f ),
                    new Vector3( -0.688191f, -0.587785f, -0.425325f )
            };
            var stream = br.BaseStream;
            stream.Position = stream.Position - mdl.Length;

            ReadHeader(mdl, br);
            ReadTextures(mdl, palette, br);
            ReadTextureCoords(mdl, br);
            ReadTriangles(mdl, br);
            //ReadVertices(mdl, br);
            ReadFrames(mdl, br);
        }

        private void ReadHeader(byte[] mdl, BinaryReader br)
        {
            Header = new MdlHeader
            {
                magic = br.ReadChars(4)
            };
            string magic = new string(Header.magic);
            if (magic != "IDPO")
            {
                return;
            }
            Header.version = br.ReadInt32();
            Vector3 scale = new Vector3();
            scale.X = br.ReadSingle();
            scale.Y = br.ReadSingle();
            scale.Z = br.ReadSingle();
            Header.scale = scale;
            Vector3 pos = new Vector3();
            pos.X = br.ReadSingle();
            pos.Y = br.ReadSingle();
            pos.Z = br.ReadSingle();
            Header.translate = pos;
            Header.boundingRadius = br.ReadSingle();
            Vector3 eyePos = new Vector3();
            eyePos.X = br.ReadSingle();
            eyePos.Y = br.ReadSingle();
            eyePos.Z = br.ReadSingle();
            Header.eyepos = eyePos;
            Header.numSkins = br.ReadInt32();
            Header.skinWidth = br.ReadInt32();
            Header.skinHeight = br.ReadInt32();
            Header.numVerts = br.ReadInt32();
            Header.numTris = br.ReadInt32();
            Header.numFrames = br.ReadInt32();
            Header.syncType = br.ReadInt32();
            Header.flags = br.ReadInt32();
            Header.size = br.ReadSingle();
        }

        private void ReadTextures(byte[] mdl, PaletteLmp palette, BinaryReader br)
        {
            Textures = new List<BitmapDataPackage>();
            for (int i = 0; i < Header.numSkins; i++)
            {
                var type = br.ReadInt32();
                bool isAnimated = type != 0 ? true : false;
                int numFrames = 0;
                float duration = 0.0f;
                if (isAnimated)
                {
                    numFrames = br.ReadInt32();
                    duration = br.ReadSingle();
                }
                var buffer = br.ReadBytes(Header.skinWidth * Header.skinHeight);
                //var texture = new GfxImg(buffer, palette, QTextureType.Skin, Header.skinWidth, Header.skinHeight, isAnimated, numFrames, duration);
                var texture = TextureHelper.ExtractSKBitmapFromLMP(buffer, QuakeFileType.Unknown, palette, Header.skinWidth, Header.skinHeight);
                if (texture != null)
                {
                    Textures.Add(texture);
                }
            }
        }

        private void ReadTextureCoords(byte[] mdl, BinaryReader br)
        {
            TextureCoords = new List<QTextureCoord>();
            for (int i = 0; i < Header.numVerts; i++)
            {
                QTextureCoord texCoord = new QTextureCoord()
                {
                    OnSeam = br.ReadInt32() == 32 ? true : false,
                    S = br.ReadInt32(),
                    T = br.ReadInt32()
                };
                TextureCoords.Add(texCoord);
            }
        }

        private void ReadTriangles(byte[] mdl, BinaryReader br)
        {
            Triangles = new List<QTriangle>();
            for (int i = 0; i < Header.numTris; i++)
            {
                QTriangle tri = new QTriangle();
                tri.FrontFacing = br.ReadInt32() == 1 ? true : false;
                tri.VertexIndex = new List<int>();
                for (int t = 0; t < 3; t++)
                {
                    tri.VertexIndex.Add(br.ReadInt32());
                }
                Triangles.Add(tri);
            }
        }
        private void ReadFrames(byte[] mdl, BinaryReader br)
        {
            Frames = new List<QMdlFrame>();
            for (int i = 0; i < Header.numFrames; i++)
            {
                int type = br.ReadInt32();
                QMdlFrame newFrame;
                if (type != 0)
                {
                    newFrame = new QMdlGroupFrame
                    {
                        Type = type,
                        MinFrame = new QVertex
                        {
                            Vertices = new List<byte> {
                                br.ReadByte(),
                                br.ReadByte(),
                                br.ReadByte()
                            },
                            Normal = br.ReadByte()
                        },
                        MaxFrame = new QVertex
                        {
                            Vertices = new List<byte> {
                                br.ReadByte(),
                                br.ReadByte(),
                                br.ReadByte()
                            },
                            Normal = br.ReadByte()
                        },
                        Duration = br.ReadSingle()
                    };

                }
                else
                {
                    newFrame = new QMdlFrame();
                }
                newFrame.Frame = new QMdlFrameSimple
                {
                    BBoxMin = new QVertex
                    {
                        Vertices = new List<byte> {
                            br.ReadByte(),
                            br.ReadByte(),
                            br.ReadByte()
                        },
                        Normal = br.ReadByte()
                    },
                    BBoxMax = new QVertex
                    {
                        Vertices = new List<byte> {
                            br.ReadByte(),
                            br.ReadByte(),
                            br.ReadByte()
                        },
                        Normal = br.ReadByte()
                    },
                    Name = new string(br.ReadChars(16)),
                    Verts = new List<QVertex>()
                };
                int index = newFrame.Frame.Name.IndexOf('\0');

                //if progs.dat don't worry about it
                if (index != -1)
                {
                    newFrame.Frame.Name = newFrame.Frame.Name.Substring(0, index);
                }
                for (int v = 0; v < Header.numVerts; v++)
                {
                    QVertex vert = new QVertex
                    {
                        Vertices = new List<byte>() {
                            br.ReadByte(),
                            br.ReadByte(),
                            br.ReadByte()
                        },
                        Normal = br.ReadByte()
                    };
                    newFrame.Frame.Verts.Add(vert);
                }
                Frames.Add(newFrame);
            }
        }
    }
}
