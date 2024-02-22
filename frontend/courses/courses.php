<?php
session_start();

// Проверяем, был ли пользователь авторизован
if (!isset($_SESSION['username'])) {
    // Если пользователь не авторизован, перенаправляем его на страницу авторизации
    header("Location: ../index.php");
    exit;
}

// Путь к файлу для записи сообщений
$contactsFile = 'contacts.txt';

// Обработка отправки формы сообщения
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Получаем данные из формы
    $name = $_POST['name'];
    $email = $_POST['email'];
    $message = $_POST['message'];

    // Создаем сообщение для записи в файл
    $contactData = "Name: $name\nEmail: $email\nMessage: $message\n\n";

    // Открываем файл для добавления нового сообщения
    $file = fopen($contactsFile, "a");
    if ($file) {
        // Записываем данные в файл
        fwrite($file, $contactData);
        
        // Закрываем файл
        fclose($file);

        // Выводим сообщение об успешной отправке
        echo "success";
    } else {
        // Выводим сообщение об ошибке, если файл не удалось открыть
        echo "error";
    }
    exit; // Прекращаем выполнение скрипта после отправки ответа AJAX
}
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Курсы - МГТУ им. Баумана</title>
    <link rel="stylesheet" href="../css/bootstrap.min.css">
    <link rel="stylesheet" href="../css/styles.css">
    <link rel="icon" href="../images/favicon.ico" type="image/x-icon">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script>
       $(document).ready(function() {
        // Функция для отправки данных формы при добавлении курса
        $("#add_course_form").submit(function(event) {
            event.preventDefault(); // Предотвращаем стандартное поведение отправки формы
            
            // Получаем данные из формы
            var formData = $(this).serialize();
            
            // Отправляем асинхронный POST-запрос на сервер для добавления курса
            $.ajax({
                type: "POST",
                url: "add_course.php",
                data: formData,
                success: function(response) {
                    // При успешном добавлении курса обновляем таблицу курсов
                    $(".table-container").load(location.href + " .table-container", function() {
                        // Очищаем поля формы после успешного добавления курса
                        $("#add_course_form")[0].reset();
                    });
                }
            });
        });

        // Обработчик клика по кнопке "Удалить"
        $(document).on("click", ".delete-course", function() {
            var courseData = $(this).data('course'); // Получаем данные строки курса
            var row = $(this).closest("tr"); // Получаем строку таблицы для удаления после успешного удаления из базы данных
            
            // Отправляем AJAX-запрос на сервер для удаления курса
            $.ajax({
                type: "POST",
                url: "delete_course.php",
                data: { courseData: courseData },
                success: function(response) {
                    if (response == "success") {
                        // Если удаление прошло успешно, удаляем строку таблицы без перезагрузки страницы
                        row.remove();
                    } else {
                        alert("Ошибка при удалении курса.");
                    }
                }
            });
        });
    });

    </script>
</head>
<body>
    <header>
        <h1>МГТУ им. Баумана</h1>
        <img src="../images/university_icon.png" alt="МГТУ им. Баумана">
    </header>
    <nav>
        <ul>
            <li><a href="../index.php">Главная</a></li>
            <li><a href="../about/about.php"> О нас</a></li>
            <li><a href="courses.php">Курсы</a></li>
            <li><a href="../contact/contact.php">Контакты</a></li>
            <li><a href="../security/logout.php">Выход</a></li>
        </ul>
    </nav>
    <main>
        <h2>Добавить курс</h2>
        <!-- Форма для добавления курса -->
        <form id="add_course_form" method="post">
            <div class="mb-3">
                <label for="course" class="form-label">Курс:</label>
                <input type="text" class="form-control" id="course" name="course" required>
            </div>
            <div class="mb-3">
                <label for="description" class="form-label">Описание:</label>
                <textarea class="form-control" id="description" name="description" rows="4" required></textarea>
            </div>
            <div class="mb-3">
                <label for="duration" class="form-label">Продолжительность:</label>
                <input type="text" class="form-control" id="duration" name="duration" required>
            </div>
            <div class="mb-3">
                <label for="instructors" class="form-label">Преподаватели:</label>
                <input type="text" class="form-control" id="instructors" name="instructors" required>
            </div>
            <button type="submit" class="btn btn-primary">Добавить курс</button>
        </form>

        <!-- Контейнер для таблицы -->
            <!-- Контейнер для таблицы -->
            <div class="table-container">
                <h2>Курсы</h2>
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th scope="col">Курс</th>
                            <th scope="col">Описание</th>
                            <th scope="col">Продолжительность</th>
                            <th scope="col">Преподаватели</th>
                            <th scope="col">Действия</th> <!-- Добавляем столбец для кнопок действий -->
                        </tr>
                    </thead>
                    <tbody>
                        <?php
                        // Считываем данные из файла courses.txt и выводим каждый курс в виде строки таблицы
                        $file = fopen("courses.txt", "r");
                        if ($file) {
                            while (($line = fgets($file)) !== false) {
                                $data = explode('|', $line);
                                echo "<tr>";
                                echo "<td>$data[0]</td>";
                                echo "<td>$data[1]</td>";
                                echo "<td>$data[2]</td>";
                                echo "<td>$data[3]</td>";
                                echo "<td><button class='btn btn-danger btn-sm delete-course' data-course='$line'>Удалить</button></td>"; // Добавляем кнопку "Удалить"
                                echo "</tr>";
                            }
                            fclose($file);
                        }
                        ?>
                    </tbody>
                </table>
            </div>

            <!-- <script>
               $(document).ready(function() {
                // Обработчик клика по кнопке "Удалить"
                $(".delete-course").click(function() {
                    var courseData = $(this).data('course'); // Получаем данные строки курса
                    var row = $(this).closest("tr"); // Получаем строку таблицы для удаления после успешного удаления из базы данных
                    
                    // Отправляем AJAX-запрос на сервер для удаления курса
                    $.ajax({
                        type: "POST",
                        url: "delete_course.php",
                        data: { courseData: courseData },
                        success: function(response) {
                            if (response == "success") {
                                // Если удаление прошло успешно, удаляем строку таблицы без перезагрузки страницы
                                row.remove();
                            } else {
                                alert("Ошибка при удалении курса.");
                            }
                        }
                    });
                });
            });

            </script> -->

    </main>
    <footer>
        &copy; 2024 МГТУ им. Баумана. Все права защищены.
    </footer>
</body>
</html>
