using PakExplorer.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public enum BSPPlaneType
    {
        Axial_X,
        Axial_Y,
        Axial_Z,
        NonAxial_X,
        NonAxial_Y,
        NonAxial_Z
    }
    public class BSPEntry
    {
        public int offset { get; set; }
        public int size { get; set; }
        public int count { get; set; }
    }

    public class BSPFace
    {
        public int PlaneID { get; set; }
        public int Side { get; set; }
        public int ListEdgeId { get; set; }
        public int ListEdgeNum { get; set; }
        public int TextureInfoId { get; set; }
        public byte LightType { get; set; }
        public int LightValue { get; set; }
        public int Light1 { get; set; }
        public int Light2 { get; set; }
        public int LightmapID { get; set; }
    }

    //public class BSPLightmap
    //{
    //    public int Index { get; set; }
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //    public byte[] Data { get; set; }
    //}

    public class BSPLightmap
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Data { get; set; }
        public List<Vector2> Vertices { get; set; }
    }

    public class BSPPlane
    {
        public int Index { get; set; }
        public Vector3 Normal { get; set; }
        public float Dist { get; set; }
        public BSPPlaneType Type { get; set; }
    }

    public class BSPBBoxShort
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MinZ { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int MaxZ { get; set; }
    }

    public class BSPBBox
    {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MinZ { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MaxZ { get; set; }
    }

    public class BSPNode
    {
        public int PlaneId { get; set; }
        public bool Front { get; set; }
        public int FrontIndex { get; set; }
        public bool Back { get; set; }
        public int BackIndex { get; set; }
        public BSPBBoxShort BBox { get; set; }
        public int FaceId { get; set; }
        public int FaceNum { get; set; }
    }

    public class BSPClipNode
    {
        public uint PlaneID { get; set; }
        public int Front { get; set; }
        public int Back { get; set; }
    }

    public class BSPTextureInfo
    {
        public Vector3 VectorS { get; set; }
        public float DistS { get; set; }
        public Vector3 VectorT { get; set; }
        public float DistT { get; set; }
        public int TextureId { get; set; }
        public bool IsAnimated { get; set; }
    }

    public class BSPEdge
    {
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }
    }

    public class BSPModel
    {
        public BSPBBox Bound { get; set; }
        public Vector3 Origin { get; set; }
        public int NodeID0 { get; set; }
        public int NodeID1 { get; set; }
        public int NodeID2 { get; set; }
        public int NodeID3 { get; set; }
        public int NumLeaves { get; set; }
        public int FaceID { get; set; }
        public int NumFaces { get; set; }
    }

    public class BSPEntity
    {
        public string Classname { get; set; }
        public Vector3 Origin { get; set; }
        public float Angle { get; set; }
        public float Speed { get; set; }
        public List<KeyValuePair<string, string>> Properties { get; set; }
    }

    public class GfxBSPHeader
    {
        public int version { get; set; }
        public BSPEntry Entities { get; set; }
        public BSPEntry Planes { get; set; }
        public BSPEntry MipTextures { get; set; }
        public BSPEntry Vertices { get; set; }
        public BSPEntry VisList { get; set; }
        public BSPEntry Nodes { get; set; }
        public BSPEntry TextureInfo { get; set; }
        public BSPEntry Faces { get; set; }
        public BSPEntry Lightmaps { get; set; }
        public BSPEntry ClipNodes { get; set; }
        public BSPEntry Leaves { get; set; }
        public BSPEntry FaceList { get; set; }
        public BSPEntry Edges { get; set; }
        public BSPEntry EdgesList { get; set; }
        public BSPEntry Models { get; set; }
    }

    public class BSPLeaf
    {
        public int Type { get; set; }
        public int VisList { get; set; }
        public BSPBBoxShort BBox { get; set; }
        public int LFaceId { get; set; }
        public int NumLeafFaces { get; set; }
        public byte SndWater { get; set; }
        public byte SndSky { get; set; }
        public byte SndSlime { get; set; }
        public byte SndLava { get; set; }
    }

    public class GfxBSPLoader
    {
        public GfxBSPHeader Header { get; set; }

        //private string _entityString { get; set; }
        public List<BSPEntity> Entities { get; set; }
        public Dictionary<string, SKBitmap> WallTextures { get; set; }
        public Dictionary<int, string> TextureIndices { get; set; }
        public List<BSPTextureInfo> TextureInfo { get; set; }
        public List<BSPEdge> Edges { get; set; }

        public List<BSPClipNode> ClipNodes { get; set; }
        public List<BSPLeaf> Leaves { get; set; }

        public List<BSPFace> Faces { get; set; }
        public List<BSPPlane> Planes { get; set; }
        public List<Vector3> Vertices { get; set; }
        public List<BSPNode> Nodes { get; set; }
        public List<byte> Lightmaps { get; set; }
        public List<int> EdgeList { get; set; }
        public byte[] VisList { get; set; }
        public List<int> FaceList { get; set; }
        public List<BSPModel> Models { get; set; }
        private long _baseStartPos { get; set; }

        public GfxBSPLoader(byte[] bspData, PaletteLmp palette, BinaryReader br)
        {
            var stream = br.BaseStream;
            stream.Position = stream.Position - bspData.Length;
            _baseStartPos = stream.Position;
            ReadHeader(bspData, br);
            resetStream(br, Header.Entities.offset);
            ReadEntities(bspData, br);
            resetStream(br, Header.Planes.offset);
            ReadPlanes(bspData, br);
            resetStream(br, Header.MipTextures.offset);
            ReadMipMaps(bspData, palette, br);
            resetStream(br, Header.Vertices.offset);
            ReadVertices(bspData, br);
            resetStream(br, Header.VisList.offset);
            ReadVisList(bspData, br);
            resetStream(br, Header.Nodes.offset);
            ReadNodes(bspData, br);
            resetStream(br, Header.TextureInfo.offset);
            ReadTextureInfo(bspData, br);
            resetStream(br, Header.Faces.offset);
            ReadFaces(bspData, br);
            resetStream(br, Header.Lightmaps.offset);
            ReadLightmaps(bspData, br);
            resetStream(br, Header.FaceList.offset);
            ReadFacesList(bspData, br);
            resetStream(br, Header.ClipNodes.offset);
            ReadClipNodes(bspData, br);
            resetStream(br, Header.Leaves.offset);
            ReadBSPLeaves(bspData, br);
            resetStream(br, Header.Edges.offset);
            ReadBSPEdges(bspData, br);
            resetStream(br, Header.EdgesList.offset);
            ReadEdgeList(bspData, br);
            resetStream(br, Header.Models.offset);
            ReadModels(bspData, br);
        }

        private void resetStream(BinaryReader br, int offset)
        {
            var stream = br.BaseStream;
            stream.Position = _baseStartPos + offset;
        }

        private void ReadHeader(byte[] data, BinaryReader br)
        {
            Header = new GfxBSPHeader
            {
                version = br.ReadInt32()
            };
            //Not a quake level - maybe later we extend this to other formats
            if (Header.version != 29)
            {
                return;
            }
            Header.Entities = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Planes = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.MipTextures = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Vertices = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.VisList = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Nodes = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.TextureInfo = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Faces = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Lightmaps = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.ClipNodes = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Leaves = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.FaceList = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Edges = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.EdgesList = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
            Header.Models = new BSPEntry
            {
                offset = br.ReadInt32(),
                size = br.ReadInt32()
            };
        }

        private void ReadEntities(byte[] bspData, BinaryReader br)
        {
            var entityString = new string(br.ReadChars(Header.Entities.size));
            parseEntities(entityString);
        }

        private void parseEntities(string entityString)
        {
            var stringArray = entityString.Split('{');
            foreach (var strEntity in stringArray)
            {
                if (string.IsNullOrWhiteSpace(strEntity))
                {
                    continue;
                }
                var propArray = strEntity.Split('\"');
                if (propArray != null && propArray.Length > 0)
                {
                    if (Entities == null)
                    {
                        Entities = new List<BSPEntity>();
                    }

                    var entity = new BSPEntity();
                    entity.Properties = new List<KeyValuePair<string, string>>();

                    for (int i = 0; i < propArray.Length; i += 4)
                    {
                        if (i + 1 < propArray.Length)
                        {
                            var key = propArray[i + 1];
                            var value = propArray[i + 3];
                            switch (key.ToUpper())
                            {
                                case "CLASSNAME":
                                    entity.Classname = value;
                                    break;
                                case "ORIGIN":
                                    var originString = value.Split(' ');
                                    if (originString.Length == 3)
                                    {
                                        float x = float.Parse(originString[0]);
                                        float y = float.Parse(originString[1]);
                                        float z = float.Parse(originString[2]);
                                        entity.Origin = new Vector3(x, y, z);
                                    }
                                    break;
                                case "ANGLE":
                                    float angle = float.Parse(value);
                                    entity.Angle = angle;
                                    break;
                                default:
                                    entity.Properties.Add(new KeyValuePair<string, string>(key, value));
                                    break;
                            }
                        }
                    }
                    Entities.Add(entity);
                }

            }
        }

        private void ReadPlanes(byte[] bspDate, BinaryReader br)
        {
            const int size = 20;
            Planes = new List<BSPPlane>();
            for (int i = 0; i < Header.Planes.size / size; i++)
            {
                BSPPlane newPlane = new BSPPlane
                {
                    Index = i,
                    Normal = new Vector3
                    {
                        X = br.ReadSingle(),
                        Y = br.ReadSingle(),
                        Z = br.ReadSingle()
                    },
                    Dist = br.ReadSingle(),
                };
                var type = br.ReadInt32();
                switch (type)
                {
                    case 0:
                        newPlane.Type = BSPPlaneType.Axial_X;
                        break;
                    case 1:
                        newPlane.Type = BSPPlaneType.Axial_Y;
                        break;
                    case 2:
                        newPlane.Type = BSPPlaneType.Axial_Z;
                        break;
                    case 3:
                        newPlane.Type = BSPPlaneType.NonAxial_X;
                        break;
                    case 4:
                        newPlane.Type = BSPPlaneType.NonAxial_Y;
                        break;
                    case 5:
                        newPlane.Type = BSPPlaneType.NonAxial_Z;
                        break;
                }
                Planes.Add(newPlane);

            }
        }

        private void ReadMipMaps(byte[] bspData, PaletteLmp palette, BinaryReader br)
        {
            int numMipMaps = br.ReadInt32();
            List<int> offsets = new List<int>();
            WallTextures = new Dictionary<string, SKBitmap>();
            TextureIndices = new Dictionary<int, string>();
            for (int i = 0; i < numMipMaps; i++)
            {
                var offset = br.ReadInt32();
                offsets.Add(offset);
            }
            for (int j = 0; j < offsets.Count; j++)
            {
                resetStream(br, Header.MipTextures.offset + offsets[j]);
                var name = new string(br.ReadChars(16));
                int index = name.IndexOf('\0');
                if (index != -1)
                {
                    name = name.Substring(0, index);
                }
                TextureIndices.Add(j, name);
                var width = br.ReadInt32();
                var height = br.ReadInt32();
                var dataOffset = br.ReadInt32();
                resetStream(br, Header.MipTextures.offset + offsets[j] + dataOffset);
                var data = br.ReadBytes(width * height);
                //GfxImg newImg = new GfxImg(data, palette, QTextureType.MapTexture, width, height);
                var newImg = TextureHelper.ExtractSKBitmapFromLMP(data, QuakeFileType.BSP, palette, width, height);
                WallTextures.Add(name, newImg.UpdatedBitmap);
            }
        }

        private void ReadVertices(byte[] bspData, BinaryReader br)
        {
            Vertices = new List<Vector3>();
            //4 bytes per float, 3 floats per vertex : 4x3 = 12
            for (int i = 0; i < Header.Vertices.size / 12; i++)
            {
                var vec = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                Vertices.Add(vec);
            }
        }

        private void ReadVisList(byte[] bspData, BinaryReader br)
        {
            VisList = br.ReadBytes(Header.VisList.size);
        }

        private void ReadNodes(byte[] bspData, BinaryReader br)
        {
            //size = sizeof Node in "Quake/DOS land"
            const int size = 24;
            Nodes = new List<BSPNode>();
            for (int i = 0; i < Header.Nodes.size / size; i++)
            {
                BSPNode newNode = new BSPNode();
                newNode.PlaneId = br.ReadInt32();
                var frontBytes = br.ReadUInt16();
                var frontBit = (frontBytes & (1 << 15)) != 0;
                newNode.Front = frontBit;
                if (frontBit)
                {
                    frontBytes = (ushort)~frontBytes;
                }
                newNode.FrontIndex = frontBytes;
                var backBytes = br.ReadUInt16();
                var backBit = (backBytes & (1 << 15)) != 0;
                newNode.Back = backBit;
                if (backBit)
                {
                    backBytes = (ushort)~backBytes;
                }
                newNode.BackIndex = backBytes;
                BSPBBoxShort bbox = new BSPBBoxShort
                {
                    MinX = br.ReadInt16(),
                    MinY = br.ReadInt16(),
                    MinZ = br.ReadInt16(),
                    MaxX = br.ReadInt16(),
                    MaxY = br.ReadInt16(),
                    MaxZ = br.ReadInt16()
                };
                newNode.BBox = bbox;

                newNode.FaceId = br.ReadInt16();
                newNode.FaceNum = br.ReadInt16();

                Nodes.Add(newNode);
            }
        }

        private void ReadTextureInfo(byte[] bspData, BinaryReader br)
        {
            TextureInfo = new List<BSPTextureInfo>();
            //size = sizeof TextureInfo in "Quake/DOS land"
            const int size = 40;
            for (int i = 0; i < Header.TextureInfo.size / size; i++)
            {
                BSPTextureInfo texInfo = new BSPTextureInfo
                {
                    VectorS = new Vector3
                    {
                        X = br.ReadSingle(),
                        Y = br.ReadSingle(),
                        Z = br.ReadSingle()
                    },
                    DistS = br.ReadSingle(),
                    VectorT = new Vector3
                    {
                        X = br.ReadSingle(),
                        Y = br.ReadSingle(),
                        Z = br.ReadSingle()
                    },
                    DistT = br.ReadSingle(),
                    TextureId = br.ReadInt32(),
                    IsAnimated = br.ReadInt32() == 1 ? true : false
                };
                TextureInfo.Add(texInfo);
            }
        }

        private void ReadFaces(byte[] bspData, BinaryReader br)
        {
            Faces = new List<BSPFace>();
            //size = sizeof Faces in "Quake/DOS land"
            const int size = 20;
            for (int i = 0; i < Header.Faces.size / size; i++)
            {
                BSPFace newFace = new BSPFace
                {
                    PlaneID = br.ReadUInt16(),
                    Side = br.ReadUInt16(),
                    ListEdgeId = br.ReadInt32(),
                    ListEdgeNum = br.ReadUInt16(),
                    TextureInfoId = br.ReadUInt16(),
                    LightType = br.ReadByte(),
                    LightValue = br.ReadByte(),
                    Light1 = br.ReadByte(),
                    Light2 = br.ReadByte(),
                    LightmapID = br.ReadInt32()
                };
                Faces.Add(newFace);
            }
        }

        private void ReadLightmaps(byte[] bspData, BinaryReader br)
        {
            //Well not sure because there's no good documentation but I think this might be legit.
            //LightmapId is just a pointer into the first pixel/texel in the array, then the "texture"
            //is built by determining the height and width. The next lightmapId would be then end of the
            //pixel array, not the next actual texture
            Lightmaps = br.ReadBytes(Header.Lightmaps.size).ToList();

        }

        private void ReadFacesList(byte[] bspData, BinaryReader br)
        {
            FaceList = new List<int>();
            var size = 2;
            for (int i = 0; i < Header.FaceList.size / size; i++)
            {
                FaceList.Add(br.ReadUInt16());
            }
        }

        private void ReadClipNodes(byte[] bspData, BinaryReader br)
        {
            ClipNodes = new List<BSPClipNode>();
            var size = 8;
            for (int i = 0; i < Header.ClipNodes.size / size; i++)
            {
                ClipNodes.Add(new BSPClipNode
                {
                    PlaneID = br.ReadUInt32(),
                    Front = br.ReadInt16(),
                    Back = br.ReadInt16()
                });
            }
        }

        private void ReadBSPLeaves(byte[] bspData, BinaryReader br)
        {
            Leaves = new List<BSPLeaf>();
            var size = 28;
            for (int i = 0; i < Header.Leaves.size / size; i++)
            {
                Leaves.Add(new BSPLeaf
                {
                    Type = br.ReadInt32(),
                    VisList = br.ReadInt32(),
                    BBox = new BSPBBoxShort
                    {
                        MinX = br.ReadInt16(),
                        MinY = br.ReadInt16(),
                        MinZ = br.ReadInt16(),
                        MaxX = br.ReadInt16(),
                        MaxY = br.ReadInt16(),
                        MaxZ = br.ReadInt16()
                    },
                    LFaceId = br.ReadUInt16(),
                    NumLeafFaces = br.ReadUInt16(),
                    SndWater = br.ReadByte(),
                    SndSky = br.ReadByte(),
                    SndSlime = br.ReadByte(),
                    SndLava = br.ReadByte()
                });
            }
        }

        private void ReadBSPEdges(byte[] bspData, BinaryReader br)
        {
            Edges = new List<BSPEdge>();
            var size = 4;
            for (int i = 0; i < Header.Edges.size / size; i++)
            {
                Edges.Add(new BSPEdge
                {
                    Vertex1 = br.ReadUInt16(),
                    Vertex2 = br.ReadUInt16()
                });
            }
        }

        private void ReadEdgeList(byte[] bspData, BinaryReader br)
        {
            EdgeList = new List<int>();
            for (int i = 0; i < Header.EdgesList.size / 4; i++)
            {
                EdgeList.Add(br.ReadInt32());
            }
        }

        private void ReadModels(byte[] bspData, BinaryReader br)
        {
            Models = new List<BSPModel>();
            var size = 64;
            for (int i = 0; i < Header.Models.size / size; i++)
            {
                Models.Add(new BSPModel
                {
                    Bound = new BSPBBox
                    {
                        MinX = br.ReadSingle(),
                        MinY = br.ReadSingle(),
                        MinZ = br.ReadSingle(),
                        MaxX = br.ReadSingle(),
                        MaxY = br.ReadSingle(),
                        MaxZ = br.ReadSingle()
                    },
                    Origin = new Vector3(br.ReadSingle(),
                        br.ReadSingle(),
                        br.ReadSingle()
                    ),
                    NodeID0 = br.ReadInt32(),
                    NodeID1 = br.ReadInt32(),
                    NodeID2 = br.ReadInt32(),
                    NodeID3 = br.ReadInt32(),
                    NumLeaves = br.ReadInt32(),
                    FaceID = br.ReadInt32(),
                    NumFaces = br.ReadInt32()
                });
            }
        }
    }
}
