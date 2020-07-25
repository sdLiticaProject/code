create table `sdliticadb`.`timeseriesmetadata` (
  `id` VARCHAR(36) NOT NULL,
  `name` VARCHAR(40) NOT NULL,
  `description` VARCHAR(40),
  `influxid` VARCHAR(36) NOT NULL,
  `datecreated` DATETIME,
  `datemodified` DATETIME,
  `userid` VARCHAR(36) NOT NULL,
  `rowscount` TINYINT,
  `columnscount` TINYINT,
  `columns` VARCHAR(80),
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_timeseriesmetadata_to_user`
    FOREIGN KEY(`userid`)
    REFERENCES `sdliticadb`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT
) ENGINE = InnoDB;