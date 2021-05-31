import sys
import os
import json
from pathlib import Path
repo_root = Path().resolve()
sys.path.append(str(repo_root))
sys.path.append(os.path.abspath(".."))

from utils.db_utils import sdLiticaInfluxClient


def LinearApproximationCallback(ch, method, properties, body):
    print('------received operation------')
    message_body = json.loads(json.loads(body)['Body'])
    print(message_body)
    operation = message_body['Operation']
    print('------------------------------')
    influx_client = sdLiticaInfluxClient()
    result = influx_client.query_timeseries(operation['TimeSeriesId'])
    print(result)

def PolynomialApproximationCallback(ch, method, properties, body):
    pass

def LogApproximationCallback(ch, method, properties, body):
    pass

def ExponentialApproximationCallback(ch, method, properties, body):
    pass


approximation_methods = {
    '*.approximation_linear': LinearApproximationCallback,
    '*.approximation_polynomial': PolynomialApproximationCallback,
    '*.approximation_log': LogApproximationCallback,
    '*.approximation_exponential': ExponentialApproximationCallback,
}
