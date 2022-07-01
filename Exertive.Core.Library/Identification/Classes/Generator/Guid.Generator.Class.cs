namespace Exertive.Core.Identification
{

    #region Dependencies

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    #endregion Dependencies

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Preference")]
    public class GuidGenerator
    {

        private readonly Guid _UrlGuid = new("6ba7b811-9dad-11d1-80b4-00c04fd430c8");

        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable ReturnTypeCanBeEnumerable.Local

        private class NetGuid
        {

            /// Bytes 0-3
            /// Unsigned 32-bit integer representing the low field of the
            /// bit integer timestamp.
            internal byte[] TimeLow { get; set; }

            /// Bytes 4-5
            /// Unsigned 16-bit integer representing the middle field of the
            /// bit integer timestamp
            internal byte[] TimeMid { get; set; }

            /// Bytes 6-7
            /// Unsigned 16-bit integer representing the high field of the
            /// bit integer timestamp multiplexed with the version number.
            internal byte[] TimeHigh { get; set; }

            /// Byte 8
            /// Unsigned 8-bit integer representing the high field of the
            /// bit integer clock sequence multiplexed with the variant.
            internal byte[] ClockHigh { get; set; }

            /// Byte 9
            /// Unsigned 8-bit integer representing the low field of the
            /// bit integer clock sequence.
            internal byte[] ClockLow { get; set; }

            /// Bytes 10-15
            /// Unsigned 48-bit integer representing the spatially unique
            /// bit integer node identifier.
            internal byte[] Node { get; set; }

            #region Constructor

            public NetGuid()
            {
                this.TimeLow = new byte[] { 0, 0, 0, 0 };
                this.TimeMid = new byte[] { 0, 0 };
                this.TimeHigh = new byte[] { 0, 0 };
                this.ClockHigh = new byte[] { 0 };
                this.ClockLow = new byte[] { 0 };
                this.Node = new byte[] { 0, 0, 0, 0, 0, 0 };
            }

            public NetGuid(byte[] bytes, bool littleEndian = false)
            {
                this.TimeLow = littleEndian ? this.ToNetworkOrder(bytes.Skip(0).Take(4).ToArray()) : bytes.Skip(0).Take(4).ToArray();
                this.TimeMid = littleEndian ? this.ToNetworkOrder(bytes.Skip(4).Take(2).ToArray()) : bytes.Skip(4).Take(2).ToArray();
                this.TimeHigh = littleEndian ? this.ToNetworkOrder(bytes.Skip(6).Take(2).ToArray()) : bytes.Skip(6).Take(2).ToArray();
                this.ClockHigh = bytes.Skip(8).Take(1).ToArray();
                this.ClockLow = bytes.Skip(9).Take(1).ToArray();
                this.Node = bytes.Skip(10).Take(6).ToArray();
            }


            public NetGuid(Guid guid) : this(guid.ToByteArray())
            {
            }

            #endregion Constructor

            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append('{');
                builder.AppendJoin('-',
                    this.ToHexString(this.TimeLow),
                    this.ToHexString(this.TimeMid),
                    this.ToHexString(this.TimeHigh),
                    this.ToHexString(this.ClockHigh),
                    this.ToHexString(this.ClockLow),
                    this.ToHexString(this.Node));
                builder.Append('}');
                return builder.ToString().ToLower();
            }

            internal string ToHexString(byte[] bytes)
            {
                return String.Join("", bytes.Select((element) => element.ToString("X2")).ToArray());
            }

            internal byte[] ToByteArray()
            {
                return this.TimeLow
                    .Concat(this.TimeMid)
                    .Concat(this.TimeHigh)
                    .Concat(this.ClockHigh)
                    .Concat(this.ClockLow)
                    .Concat(this.Node)
                    .ToArray();
            }

            internal byte[] ToLocalByteArray()
            {
                return FromNetworkOrder(this.TimeLow)
                    .Concat(FromNetworkOrder(this.TimeMid))
                    .Concat(FromNetworkOrder(this.TimeHigh))
                    .Concat(this.ClockHigh)
                    .Concat(this.ClockLow)
                    .Concat(this.Node).ToArray();
            }

            /// <summary>
            /// Converts the bytes provided to network big-endian order depending on the 'endianness'
            /// of the host machine. This means that the byte order is reversed if the host machine
            /// is little endian as is Windows.
            /// </summary>
            /// <param name="bytes">The bytes to convert to network byte order.</param>
            /// <returns>The suitably converted array of bytes.</returns>
            [DebuggerStepThrough]
            internal byte[] ToNetworkOrder(byte[] bytes)
            {
                return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes;
            }

            /// <summary>
            /// Converts the bytes provided to the host machine order depending on the 'endianness'
            /// of the host machine. This means that the byte order is reversed if the host machine
            /// is little endian as is Windows.
            /// </summary>
            /// <param name="bytes">The bytes to convert to network byte order.</param>
            /// <returns>The suitably converted array of bytes.</returns>
            [DebuggerStepThrough]
            internal byte[] FromNetworkOrder(byte[] bytes)
            {
                return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes;
            }

            [DebuggerStepThrough]
            internal byte[] And(IReadOnlyCollection<byte> value, IReadOnlyCollection<byte> mask)
            {
                Debug.Assert(value != null && mask != null);
                Debug.Assert(value.Count == mask.Count);
                return value.Select((element, index) => element &= mask.ElementAt(index)).ToArray();
            }

            [DebuggerStepThrough]
            internal byte[] Or(IReadOnlyCollection<byte> value, IReadOnlyCollection<byte> mask)
            {
                Debug.Assert(value != null && mask != null);
                Debug.Assert(value.Count == mask.Count);
                return value.Select((element, index) => element |= mask.ElementAt(index)).ToArray();
            }

        }


        public GuidGenerator()
        {
        }

        // ReSharper restore MemberCanBePrivate.Local
        // ReSharper restore ReturnTypeCanBeEnumerable.Local


        public Guid GenerateUriGuid(string name, int version = 5)
        {

            // Convert the domain or namespace Guid to network byte order so it produces
            // the same hash key regardless of the byte order of the machine on which the
            // code is run.

            var nameBytes = Encoding.UTF8.GetBytes(name);
            var namespaceGuid = new NetGuid(this._UrlGuid.ToByteArray(), true);
            var namespaceBytes = namespaceGuid.ToByteArray();

            byte[] hash;
            using (var algorithm = (HashAlgorithm)(version == 5 ? SHA1.Create() : MD5.Create()))
            {
                var bytes = new byte[namespaceBytes.Length + nameBytes.Length];
                Buffer.BlockCopy(namespaceBytes, 0, bytes, 0, namespaceBytes.Length);
                Buffer.BlockCopy(nameBytes, 0, bytes, namespaceBytes.Length, nameBytes.Length);
                hash = algorithm.ComputeHash(bytes);
            }

            // Copy the first 16 bytes of the 20-byte hash key to a byte array from which
            // to construct the GUID.
            var uriBytes = new byte[16];
            Array.Copy(hash, 0, uriBytes, 0, 16);

            // Set the four most significant bits (bits 12 through 15) of the TimeHigh
            // field to the appropriate 4-bit version number.
            uriBytes[6] = (byte)(uriBytes[6] & 0x0F | version << 4);

            // Set the two most significant bits (bits 6 and 7) of the ClockHigh field
            // to zero and one, respectively.
            uriBytes[8] = (byte)(uriBytes[8] & 0x3F | 0x80);

            // Convert the resuUsage UUID to local byte order
            uriBytes = SwapByteOrder(uriBytes);
            return new Guid(uriBytes);
        }

        private byte[] SwapByteOrder(byte[] bytes)
        {
            var segments = new byte[4][];
            segments[0] = bytes.Skip(0).Take(4).Reverse().ToArray();
            segments[1] = bytes.Skip(4).Take(2).Reverse().ToArray();
            segments[2] = bytes.Skip(6).Take(2).Reverse().ToArray();
            segments[3] = bytes.Skip(8).Take(8).ToArray();
            return ConcatenateByteArrays(segments);

            //return bytes.Skip(0).Take(4).Reverse()
            //    .Concat(bytes.Skip(4).Take(2).Reverse())
            //    .Concat(bytes.Skip(6).Take(2).Reverse())
            //    .Concat(bytes.Skip(8).Take(8))
            //    .ToArray();
        }

        private byte[] ConcatenateByteArrays(params byte[][] arrays)
        {
            IEnumerable<byte> bytes = Array.Empty<byte>();
            foreach (var array in arrays)
            {
                bytes = bytes.Concat(array);
            }
            return bytes.ToArray();
        }

    }
}
