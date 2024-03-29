create table `DASHBOARDS` (
  `ID` VARCHAR(36) NOT NULL,
  `TITLE` VARCHAR(40) NOT NULL,
  `DESCRIPTION` VARCHAR(40),
  `USER_ID` VARCHAR(36) NOT NULL,
  CONSTRAINT `DASHBOARDS_PK`
    PRIMARY KEY (`ID`),
  CONSTRAINT `DASHBOARDS_USER_FK`
    FOREIGN KEY(`USER_ID`)
    REFERENCES `USERS` (`ID`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT
) ENGINE = InnoDB;

create table `WIDGETS` (
  `ID` VARCHAR(36) NOT NULL,
  `TITLE` VARCHAR(40) NOT NULL,
  `DESCRIPTION` VARCHAR(40),
  `DASHBOARD_ID` VARCHAR(36) NOT NULL,
  `TYPE` VARCHAR(40) NOT NULL,
  `TIMESERIES_ID` VARCHAR(36) NOT NULL,
  `ANALYTICS_ID` VARCHAR(36),
  `ARGUMENTS` JSON,
  CONSTRAINT `DASHBOARDS_PK`
    PRIMARY KEY (`ID`),
  CONSTRAINT `WIDGET_DASHBOARDS_FK`
    FOREIGN KEY(`DASHBOARD_ID`)
    REFERENCES `DASHBOARDS` (`ID`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `WIDGET_TIMESERIES_FK`
    FOREIGN KEY(`TIMESERIES_ID`)
    REFERENCES `TIMESERIES_METADATA` (`ID`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT
) ENGINE = InnoDB;