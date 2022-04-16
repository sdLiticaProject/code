class InfluxConfig:
    host = 'localhost'
    port = 8086
    username = 'root'
    password = 'root'
    database = 'sdLitica'

class AMQPConfig:
    host = 'localhost'
    port = 5672

class AliveSignalConfig:
    interval = 5
    daemon = True