-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: mariadb-fonosoft
-- Tiempo de generación: 02-12-2024 a las 16:42:25
-- Versión del servidor: 11.6.2-MariaDB
-- Versión de PHP: 8.2.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `fonosoft`
--
CREATE DATABASE IF NOT EXISTS `fonosoft` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci;
USE `fonosoft`;

DELIMITER $$
--
-- Procedimientos
--
DROP PROCEDURE IF EXISTS `I_Usuario`$$
CREATE DEFINER=`root`@`%` PROCEDURE `I_Usuario` (IN `pNombreUsuario` TEXT, IN `pEmail` TEXT, IN `pContrasenia` TEXT)   BEGIN
    INSERT INTO usuario(
        nombre_usuario
        , contrasenia
        ,email
    )
    VALUES(
        pNombreUsuario
        ,pContrasenia
        ,pEmail
    );

    SELECT LAST_INSERT_ID() id;
END$$

DROP PROCEDURE IF EXISTS `S_UsuarioXNombreUsuario`$$
CREATE DEFINER=`root`@`%` PROCEDURE `S_UsuarioXNombreUsuario` (IN `pNombreUsuario` TEXT)   BEGIN
    SELECT 
        usuario.id
        ,usuario.nombre_usuario
        ,usuario.contrasenia
        ,usuario.email
    FROM usuario
    WHERE usuario.nombre_usuario = pNombreUsuario;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nombre_usuario` text NOT NULL,
  `contrasenia` text NOT NULL,
  `email` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `nombre_usuario_unique` (`nombre_usuario`) USING HASH;

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
