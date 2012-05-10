using System.Reflection;

namespace RazorEngine.Web
{
    using System;
    using System.Web;
    using System.Web.Compilation;
    using System.Web.Razor;
    using System.Web.Razor.Parser;

    using Compilation;

    /// <summary>
    /// Defines a compiler service that used the ASP.NET BuildProvider system.
    /// </summary>
    public abstract class WebCompilerServiceBase : CompilerServiceBase
    {
        #region Constructor
        /// <summary>
        /// Initialises a new instance of <see cref="WebCompilerServiceBase"/>.
        /// </summary>
        /// <param name="virtualFileExtension">The virtual file extensions.</param>
        /// <param name="codeLanguage">The razor code language.</param>
        /// <param name="markupParser">The markup parser.</param>
        protected WebCompilerServiceBase(string virtualFileExtension, RazorCodeLanguage codeLanguage, MarkupParser markupParser)
            : base(codeLanguage, ()=>markupParser)
        {
            if (string.IsNullOrWhiteSpace(virtualFileExtension))
                throw new ArgumentException("Virtual file extension is required.");

            VirtualFileExtension = virtualFileExtension;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the file extension for the virtual file.
        /// </summary>
        public string VirtualFileExtension { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Compiles the type defined in the specified type context.
        /// </summary>
        /// <param name="context">The type context which defines the type to compile.</param>
        /// <returns>The compiled type.</returns>
        public override Tuple<Type, Assembly> CompileType(TypeContext context)
        {
            string virtualFile = "~/__razor/" + context.ClassName + "." + VirtualFileExtension;
            HttpContext.Current.Items.Add(context.ClassName, context);
            var type = BuildManager.GetCompiledType(virtualFile);
            return Tuple.Create(type, type.Assembly);
        }
        #endregion
    }
}