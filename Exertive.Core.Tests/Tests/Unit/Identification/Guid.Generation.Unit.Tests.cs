namespace Exertive.Core.Tests.Tests.Unit.Identification
{

    using System;

    using Xunit;

    using Exertive.Core.Identification;

    public class GuidGenerationUnitTest
    {

        // https://www.ietf.org/rfc/rfc4122.txt
        // https://en.wikipedia.org/wiki/Universally_unique_identifier#Versions_3_and_5_(namespace_name-based)
        // https://www.famkruithof.net/guid-uuid-namebased.html
        /// <summary>
        /// Helper methods for working with <see cref="Guid"/>.
        /// </summary>

        /// <summary>
        /// Predefined namespace to generate deterministic <see cref="Guid"/>.
        /// </summary>
        private static class Namespaces
        {
            /// <summary>
            /// The namespace for Commands.
            /// </summary>
            public static readonly Guid Commands = new("b8bfc711-ed0b-4151-a4fd-26a749825f7b");

            /// <summary>
            /// The namespace for Events.
            /// </summary>
            public static readonly Guid Events = new("115a74c3-19dd-4753-b31e-f366eb3e2005");

            /// <summary>
            /// The namespace for fully-qualified domain names (from RFC 4122, Appendix C).
            /// </summary>
            public static readonly Guid Dns = new("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

            /// <summary>
            /// The namespace for URLs (from RFC 4122, Appendix C).
            /// </summary>
            public static readonly Guid Url = new("6ba7b811-9dad-11d1-80b4-00c04fd430c8");

            /// <summary>
            /// The namespace for ISO OIDs (from RFC 4122, Appendix C).
            /// </summary>
            public static readonly Guid IsoOid = new("6ba7b812-9dad-11d1-80b4-00c04fd430c8");

            /// <summary>
            /// The namespace for X.500 DN (from RFC 4122, Appendix C).
            /// </summary>
            public static readonly Guid X500Dn = new("6ba7b814-9dad-11d1-80b4-00c04fd430c8");
        }

        [Fact]
        public void TestGenerationOfUriGuid()
        {
            var expectedGuid = Guid.Parse("c367d8f4-4e7d-5e2f-9682-f69afd71d664");
            var expectedValue = expectedGuid.ToByteArray();
            var uri = "http://schema.exertive.io/test";
            var guid = new GuidGenerator().GenerateUriGuid(uri);

            Assert.Equal(guid.ToByteArray(), expectedValue);

        }

    }
}
