<?php
session_start();
// Удаляем все данные сессии
session_unset();
// Уничтожаем сессию
session_destroy();
// Перенаправляем пользователя на главную страницу или куда-либо еще
header("Location: ../index.php");
exit;
?>
