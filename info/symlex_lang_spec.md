Σymlex Language Specification

Language Design Principles
Symbolic Expressiveness: Symbols directly map to concepts, operations, or data structures for intuitive coding.
Contextual Semantics: Semantic interpretation based on usage context, reducing ambiguity.
Dynamic Typing and Inference: Types inferred from context, enhancing code flexibility.
AI-Driven Compilation/Interpretation: AI learns from code to optimize execution.

Prefix Notation
Σymlex uses Prefix Notation (Polish Notation) for all operations to maintain consistency with its declarative approach, enhance readability, and support AI-driven parsing.

Basic Constructs

Variable Declaration:
plaintext
[=var name value]
Function/Procedure Definition:
plaintext
[=proc name 
:input parameters
:return return_type  // Optional, can be singular or multiple types
:body (code here)]
Function/Procedure Call with Lambda-like Syntax:
plaintext
[=var name () => function_name arg1 arg2 ...]  // Immediate invocation
[=var name (param) => function_name arg1 arg2 ...]  // Dynamic function declaration
[=var name (param) => function_name :with collection]  // Iteration over collection
[=var name () => function_name :with value]  // Direct value passing

Explicit Return and Function Exit:

Push Value: [push value] - Pushes a value onto the stack or into a register.
Exit Function: (esc) - Exits the function, returning all pushed values as a result.

Data Types

Byte (byte): An 8-bit signed integer.
Number (num): A 32-bit integer for general-purpose arithmetic.
Float (float): A 64-bit floating-point number for decimal calculations.
String:
Constant String (cstr) - Immutable strings for static values.
Mutable String (str) - For dynamic string manipulation.
Boolean (bool): A single bit representing true/false states, with considerations for bit packing for memory efficiency.
Enum (enum): Represents a set of named constants.
plaintext
[=enum EnumName 
:values value1, value2, ...]
Custom Types:
plaintext
[=type name :attributes]
Example:
plaintext
[=type short :size 2]  // Defines a 2-byte short integer type

Mathematical Calculation Center

Basic Arithmetic Operations:
plaintext
[=proc add 
:input a:num, b:num
:return num
:body (+ a b)]
Similar definitions for subtract, multiply, divide.

Appendix: Mathematical Operations

Matrix Declaration

Syntax for Matrix Declaration:
plaintext
[=var name :matrix [[element1 element2 ...] [element1 element2 ...] ...]]
Example:
plaintext
[=var myMatrix :matrix [[1 2 3] [4 5 6] [7 8 9]]]  // A 3x3 matrix

Linear Algebra, Calculus, Matrix Operations: (As previously defined)

Namespaces

Syntax: ns.func for accessing functions or variables in a namespace. If no namespace is specified, the entity is placed in the common namespace.

String Interpolation

Syntax: Using $ before a string to denote interpolation, with {variable} to embed variables:
plaintext
[push $"Hello, {name}!"]

Error and Exception Handling

Errors

Informational Nature: Errors are intended to inform about potential issues without stopping the program's execution.
Syntax for Reporting Errors:
plaintext
[=report error_message:str]

Exceptions

Catastrophic Nature: Exceptions indicate severe conditions where program continuation might not be safe or feasible.
Syntax for Handling Exceptions:
plaintext
[=try 
:body (code that might throw an exception)
:catch exception_type 
:handler (code to handle the exception)]

Language Growth and Evolution

Need-Based Expansion: New symbols or keywords based on programming needs, like [=map] or [=reduce].
Community Feedback: Developers can propose new symbols or grammar extensions through forums or AI-driven analysis.
AI-Driven Syntax Proposals: AI analyzes code patterns to suggest new constructs or optimizations, like [=async] for asynchronous operations.
Backward Compatibility: Maintain compatibility with existing code unless there's a strong consensus for change.
Formal Proposal and Review Process: Proposal, Community Discussion, Testing, Review, and Approval stages for new language features.
Documentation and Education: Documentation updated with each addition; educational materials created to facilitate adoption.
Performance and Security Impact Analysis: Analyze how new constructs affect system performance, security, and resource usage.
Extensibility for AI Interoperability: New symbols to support multi-AI environments, like [=inter_AI_protocol] for AI data exchange.

Examples of Potential Future Additions

Quantum Computing Primitives: [=qubit], [=measure], [=entangle].
Blockchain Operations: [=blockchain].
Enhanced Security Constructs: [=zero_knowledge_proof].

Appendix: Miscellaneous

Casting and Type Conversion

Casting cstr to str:
When assigning a cstr (constant string) to a str (mutable string), the value is copied, but the new variable takes on the characteristics of its declared type:
plaintext
[=var label :cstr "Hello"]
[=var text :const str label]  // 'text' holds "Hello" but is treated as immutable str
[=var myStr :str label]       // 'myStr' holds "Hello" as a mutable str, allowing for string operations

Appendix: Built-in Functions

Built-in Functions in Σymlex

Output:
plaintext
[=proc output 
:input value:any
:body (print value)]
Exit:
plaintext
[=proc exit 
:input status:num
:body (terminate status)]
Info:
plaintext
[=proc info 
:input key:str
:return any
:body (get_info key)]
Help:
plaintext
[=proc help 
:input command:str
:body (display_help command)]
Typeof:
plaintext
[=proc typeof 
:input value:any
:return str
:body (get_type value)]
Length:
plaintext
[=proc length 
:input collection:any
:return num
:body (size_of collection)]
Random:
plaintext
[=proc random 
:input min:num, max:num
:return num
:body (generate_random min max)]

Appendix: Practical Examples of Σymlex Constructs

Variable Declaration and Assignment

Basic Declaration:
plaintext
[=var count :num 0]  // Declaring and initializing a variable
Reassignment:
plaintext
[=var text :str "Hello"]
[=var text "World"]  // Reassigning to the same variable
Declaring with Inference:
plaintext
[=var inferred "Inferred as str"]

Function Values and Returns

Returning a Single Value:
plaintext
[=proc square 
:input x:num
:return num
:body [push (* x x)] (esc)]

[=var result :num () => square :with 5]  // result would be 25
Returning Multiple Values:
plaintext
[=proc swap 
:input a:num, b:num
:return num, num
:body [push a] [push b] (esc)]

[=var first :num, second :num () => swap :with 1 2]
// first would be 2, second would be 1 after this call

Semantics in Function Calls with Lambda Syntax

Immediate Invocation:
plaintext
[=var immediateResult :num () => add :with 3 4]  // Immediate execution of 'add'
Dynamic Function Declaration:
plaintext
[=var addFive (x:num) => add :with x 5]
[=var sum :num () => addFive :with 10]  // sum would be 15
Iteration Over Collections:
plaintext
[=var numbers :list [1 2 3 4 5]]
[=var doubled (x:num) => multiply :with x 2 :with numbers]  
// 'doubled' would be a list with each number doubled
Single Value Application:
plaintext
[=var singleValue :num 10]
[=var result (x:num) => add :with x 5 :with singleValue]  
// Here, 'result' would be 15 since 'singleValue' is passed directly to add
Anonymous Functions for Iteration:
plaintext
[=var generator () => (while (< count 5) do [push count] [=var count (+ count 1)])]
[=var sequence (x:num) => output :with x :with generator]
// This would output numbers from 0 to 4, incrementing count each iteration

Additional Semantics Notes:

Closure: Lambda expressions in Σymlex can capture variables from their surrounding scope, maintaining their values even if the scope they were defined in goes out of scope:
plaintext
