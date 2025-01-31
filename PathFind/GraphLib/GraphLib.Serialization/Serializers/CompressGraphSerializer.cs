﻿using GraphLib.Interfaces;
using GraphLib.NullRealizations.NullObjects;
using GraphLib.Serialization.Exceptions;
using GraphLib.Serialization.Interfaces;
using System;
using System.IO;
using System.IO.Compression;

namespace GraphLib.Serialization.Serializers
{
    public sealed class CompressGraphSerializer : IGraphSerializer
    {
        public CompressGraphSerializer(IGraphSerializer serializer)
        {
            this.serializer = serializer;
        }

        public IGraph LoadGraph(Stream stream)
        {
            IGraph graph = NullGraph.Instance;
            try
            {
                using (var compressionStream = new GZipStream(stream, CompressionMode.Decompress, leaveOpen: true))
                {
                    graph = serializer.LoadGraph(compressionStream);
                }
                return graph;
            }
            catch (Exception ex)
            {
                throw new CantSerializeGraphException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Saves graph in stream and compresses it
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="stream"></param>
        public void SaveGraph(IGraph graph, Stream stream)
        {
            try
            {
                using (var compressionStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true))
                {
                    serializer.SaveGraph(graph, compressionStream);
                }
            }
            catch (Exception ex)
            {
                throw new CantSerializeGraphException(ex.Message, ex);
            }
        }

        private readonly IGraphSerializer serializer;
    }
}