-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: mariadb-fonosoft
-- Tiempo de generación: 04-12-2024 a las 20:32:30
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

DELIMITER $$
--
-- Procedimientos
--
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

CREATE DEFINER=`root`@`%` PROCEDURE `S_UsuarioXNombreUsuario` (IN `pNombreUsuario` TEXT)   BEGIN
    SELECT 
        usuario.id
        ,usuario.nombre_usuario
        ,usuario.contrasenia
        ,usuario.email
    FROM usuario
    WHERE usuario.nombre_usuario = pNombreUsuario;
END$$

CREATE DEFINER=`root`@`%` PROCEDURE `U_UsuarioModifContrasenia` (IN `pId` INT, IN `pContrasenia` TEXT)   BEGIN
    UPDATE usuario 
    SET
        contrasenia = pContrasenia
    WHERE id = pId;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nombre_usuario` text NOT NULL,
  `contrasenia` text NOT NULL,
  `email` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre_usuario`, `contrasenia`, `email`) VALUES
(4, '3IHtiZ850NdywiJcBapbww==', 'TteXtSOmvEGl4PAZSbdhsQ==', 'G6DutyUwxTce+ZAbTYHQQnCxT/1uTWKpKdX095zo+rc=');

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
