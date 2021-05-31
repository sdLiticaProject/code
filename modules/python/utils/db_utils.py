from influxdb import InfluxDBClient

class sdLiticaInfluxClient():
    def __init__(self):
        self.client = InfluxDBClient('localhost', 8086, 'root', 'root', 'sdLitica')
    def query_timeseries(self, measurementId: str):
        result = self.client.query('select * from "' + measurementId + '";')
        return result
        
