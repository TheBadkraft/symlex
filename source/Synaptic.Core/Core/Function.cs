using Synaptic.Analysis;

namespace Synaptic.Core
{
    /// <summary>
    /// Represents a function with a name, parameters, body, and return type.
    /// </summary>
    public class Function
    {
        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of parameters of the function.
        /// </summary>
        public IReadOnlyCollection<Parameter> Parameters { get; private set; }

        /// <summary>
        /// Gets the body of the function represented as a list of tokens.
        /// </summary>
        public IReadOnlyCollection<Token> Body { get; private set; }

        /// <summary>
        /// Gets the return type of the function.
        /// </summary>
        public string ReturnType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="parameters">The list of parameters of the function.</param>
        /// <param name="body">The body of the function represented as a list of tokens.</param>
        /// <param name="returnType">The return type of the function.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="name"/>, <paramref name="parameters"/>, or <paramref name="body"/> is null.
        /// </exception>
        public Function(string name, IReadOnlyCollection<Parameter> parameters, IReadOnlyCollection<Token> body, string returnType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            ReturnType = returnType;
        }

        /// <summary>
        /// Represents a parameter of a function with a name and type.
        /// </summary>
        public class Parameter
        {
            /// <summary>
            /// Gets or sets the name of the parameter.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the type of the parameter.
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Parameter"/> class.
            /// </summary>
            /// <param name="name">The name of the parameter.</param>
            /// <param name="type">The type of the parameter.</param>
            public Parameter(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }
    }
}
