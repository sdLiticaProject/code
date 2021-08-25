import json
from configs.config import InfluxConfig
import sys
import os
from pathlib import Path
repo_root = Path().resolve()
sys.path.append(str(repo_root))
sys.path.append(os.path.abspath(".."))

from utils.db_utils import sdLiticaInfluxClient


def LinearApproximation123(influx_client, message_body):
    operation = message_body['Operation']
    result = influx_client.query_timeseries(operation['TimeSeriesId'])
    print(result)

def PolynomialApproximation(ch, method, properties, body):
    pass

def LogApproximation(ch, method, properties, body):
    pass

def ExponentialApproximation(ch, method, properties, body):
    pass



approximation_methods = {
    '*.approximation_lineaarr': LinearApproximation123
}
