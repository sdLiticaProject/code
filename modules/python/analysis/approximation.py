import sys
import os
from pathlib import Path
repo_root = Path().resolve()
sys.path.append(str(repo_root))
sys.path.append(os.path.abspath(".."))

from utils.db_utils import sdLiticaInfluxClient


def LinearApproximation(ch, method, properties, body):
    print(body)
    influx_client = sdLiticaInfluxClient(body.timeSeriesId)
    result = influx_client.query_timeseries()
    print(result)

def PolynomialApproximation(ch, method, properties, body):
    pass

def LogApproximation(ch, method, properties, body):
    pass

def ExponentialApproximation(ch, method, properties, body):
    pass



approximation_methods = {
    '*.approximation.linear': LinearApproximation,
    '*.approximation.polynomial': LinearApproximation,
    '*.approximation.log': LinearApproximation,
    '*.approximation.exponential': LinearApproximation,
}
