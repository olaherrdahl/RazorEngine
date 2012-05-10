namespace RazorEngine.Web
{
    using System;
    using System.Collections;
    using System.Web.Caching;
    using System.Web.Hosting;

    /// <summary>
    /// Defines a virtual path provider used to resolve dynamic compiler calls.
    /// </summary>
    public class RazorVirtualPathProvider : VirtualPathProvider
    {
        #region Methods
        /// <summary>
        /// Determines if the virtual path is a razor dynamic call.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>True if the virtual file is a dynamic razor call, otherwise false.</returns>
        private static bool IsRazorVirtualPath(string virtualPath)
        {
            return virtualPath.ToLowerInvariant().StartsWith("/__razor");
        }

        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool FileExists(string virtualPath)
        {
            return IsRazorVirtualPath(virtualPath) || Previous.FileExists(virtualPath);
        }

        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Caching.CacheDependency"/> object for the specified virtual resources.
        /// </returns>
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsRazorVirtualPath(virtualPath))
                return null;

            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
        #endregion
    }
}