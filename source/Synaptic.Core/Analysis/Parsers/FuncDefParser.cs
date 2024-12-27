
using Synaptic.Core;
using Synaptic.Analysis.Parsers;

namespace Synaptic.Analysis;

public class FuncDefParser : SynapticParser
{
    private ContextDescriptor Descriptor { get; }

    public FuncDefParser(ContextDescriptor descriptor) => Descriptor = descriptor;

    public override bool Parse()
    {
        int index = 0;
        var name = string.Empty;
        var parameters = default(IReadOnlyCollection<Function.Parameter>);
        var body = default(IReadOnlyCollection<Token>);
        var returnType = string.Empty;

        //  read the name of the function
        if (!TryParseIdentifier(ref index, Descriptor.Data, out name))
        {
            return false;    //  missing or invalid function name
        }
        //  read expected input declaration
        if (!TryConsumeToken(ref index, Descriptor.Data, TokenType.Operator, ":"))
        {
            return false;    //  missing input declaration
        }
        //  read parameters
        if (!TryParseParameters(ref index, Descriptor.Data, out parameters))
        {
            return false;    //  invalid parameters
        }
        //  read expected body declaration
        if (!TryConsumeToken(ref index, Descriptor.Data, TokenType.Operator, ":"))
        {
            return false;    //  missing input declaration
        }
        //  read body
        if (!TryParseBody(ref index, Descriptor.Data, out body))
        {
            return false;    //  invalid body
        }

        // return new Function(name, parameters, body, returnType);
        return true;
    }
}
