from jsonschema import validate


def validate_linear_interpolation(args):
    input_structure = {
        'type': 'object',
        'properties': {
            'x_column': {'type': 'string'},
            'y_column': {'type': 'string'},
            'points': {
                'type': 'array',
                'items': {
                    'type': 'number'
                }
            }
        }
    }
    validate(args, schema=input_structure)


def validate_simple_reduce(args):
    input_structure = {
        'type': 'object',
        'properties': {
            'column': {'type': 'string'},
            'function': {'type': 'string'}
        }
    }
    available_functions = ['max', 'min', 'mean']

    validate(args, schema=input_structure)
    if args['function'] not in available_functions:
        raise ValueError(f'Available reduce functions: {", ".join(available_functions)}')


def validate_linear_approximation(args):
    input_structure = {
        'type': 'object',
        'properties': {
            'x_column': {'type': 'string'},
            'y_column': {'type': 'string'}
        }
    }
    validate(args, schema=input_structure)

