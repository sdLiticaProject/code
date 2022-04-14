from configs.config import InfluxConfig
from influxdb import InfluxDBClient

class sdLiticaInfluxClient:
    def __init__(self, influx_config: InfluxConfig):
        self.client = InfluxDBClient(
            influx_config.host, 
            influx_config.port, 
            influx_config.username, 
            influx_config.password, 
            influx_config.database
        )
            
    def query_timeseries(self, measurementId: str):
        result = self.client.query('select * from "' + measurementId + '";')
        return result
        
