import json
from configs.config import InfluxConfig
import sys
import os
import numpy as np
from pathlib import Path
repo_root = Path().resolve()
sys.path.append(str(repo_root))
sys.path.append(os.path.abspath(".."))

from validations.arg_validation import *
from utils.db_utils import sdLiticaInfluxClient


def LinearApproximation(influx_client, message_body):
    operation = message_body['Operation']
    args = operation['Arguments']
    validate_linear_approximation(args)

    result = influx_client.query_timeseries(operation['TimeSeriesId'])
    points = list(result.get_points())

    xs = [p[args['x_column']] for p in points]
    ys = [p[args['y_column']] for p in points]
    result = np.polyfit(xs, ys, 1)

    print(f"{result[0]}x {'+' if result[1] > 0 else '-'} {result[1]}")


def LinearInterpolation(influx_client, message_body):
    operation = message_body['Operation']
    args = operation['Arguments']
    validate_linear_interpolation(args)

    result = influx_client.query_timeseries(operation['TimeSeriesId'])
    points = list(result.get_points())

    xs = [p[args['x_column']] for p in points]
    ys = [p[args['y_column']] for p in points]    
    targets = args['points']

    result = np.interp(targets, xs, ys)

    print(operation.tolist())


def SimpleReduce(influx_client, message_body):
    operation = message_body['Operation']
    print(operation)
    args = operation['Arguments']
    validate_simple_reduce(args)

    column = args['column']
    result = influx_client.query_timeseries(operation['TimeSeriesId'])
    points = list(result.get_points())
    series = [p[column] for p in points]
    
    if args['function'] == 'mean':
        result = np.mean(series)
    elif args['function'] == 'max':
        result = max(series)
    elif args['function'] == 'min':
        result = min(series)
    print(result)


approximation_methods = {
    '*.approximation_linear': LinearApproximation,
    '*.interpolation_linear': LinearInterpolation,
    '*.reduce': SimpleReduce,
}
