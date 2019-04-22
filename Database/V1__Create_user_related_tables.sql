CREATE TABLE `sdliticadb`.`user` (
  `id` VARCHAR(36) NOT NULL,
  `firstname` VARCHAR(45) NULL,
  `lastname` VARCHAR(45) NULL,
  `email` VARCHAR(45) NOT NULL,
  `password` VARCHAR(256) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE)
ENGINE = InnoDB;


CREATE TABLE `sdliticadb`.`user_token` (
  `id` VARCHAR(36) NOT NULL,
  `token` VARCHAR(256) NULL,
  `userid` VARCHAR(36) NOT NULL,
  `expiration` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `user_token_to_user_idx` (`userid` ASC) VISIBLE,
  CONSTRAINT `fk_user_token_to_user`
    FOREIGN KEY (`userid`)
    REFERENCES `sdliticadb`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT);
