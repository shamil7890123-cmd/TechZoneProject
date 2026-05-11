// Инициализация корзины
function initCart() {
    var buttons = document.querySelectorAll('.btn-add, .btn-to-cart');

    for (var i = 0; i < buttons.length; i++) {
        buttons[i].addEventListener('click', function () {
            alert("Товар добавлен в корзину!");

            if (this.classList.contains('btn-to-cart')) {
                this.innerText = "В корзине";
                this.style.background = "#28a745";
            }
        });
    }
}

// Проверка отзыва
function checkReview() {
    var name = prompt("Ваше имя:");
    if (name == "" || name == null) {
        alert("Введите имя!");
        return false;
    } else {
        alert("Спасибо, " + name + "!");
        return true;
    }
}

// Карточка товара: выбор фото и кнопка покупки
function initProductCard() {
    var thumbs = document.querySelectorAll('.thumb-card');
    var mainPhotoBlock = document.querySelector('.main-photo-card');
    var mainPhotoPlaceholder = document.querySelector('.main-photo-card .placeholder');

    for (var i = 0; i < thumbs.length; i++) {
        thumbs[i].onclick = function () {
            if (mainPhotoBlock) {
                var content = "<h2 style='margin: auto;'>" + this.innerText + "</h2>";
                if (mainPhotoPlaceholder) {
                    mainPhotoPlaceholder.innerText = "Выбрано: " + this.innerText;
                } else {
                    mainPhotoBlock.innerHTML = content;
                }
                alert("Вы выбрали: " + this.innerText);
            }
        };
    }

    var buyBtn = document.querySelector('.btn-buy-now');
    if (buyBtn) {
        buyBtn.onclick = function () {
            this.innerHTML = "✅ В КОРЗИНЕ";
            this.style.background = "#2e7d32";
            this.style.color = "white";
        };
    }
}

// Фильтрация в каталоге
function initCatalog() {
    var searchInput = document.querySelector('.search-input');
    var products = document.querySelectorAll('.product-item');

    if (searchInput) {
        searchInput.oninput = function () {
            var filter = searchInput.value.toLowerCase();
            for (var i = 0; i < products.length; i++) {
                var title = products[i].querySelector('h3').innerText.toLowerCase();
                products[i].style.display = title.indexOf(filter) > -1 ? "" : "none";
            }
        };
    }
}

// Валидация формы контактов
function initContacts() {
    var form = document.querySelector('.contact-form');
    if (form) {
        form.onsubmit = function () {
            var name = document.querySelector('input[type="text"]').value;
            var email = document.querySelector('input[type="email"]').value;

            if (name == "") {
                alert("Вы не ввели имя!");
                return false;
            }
            if (email.indexOf("@") == -1) {
                alert("Ошибка в почте!");
                return false;
            }
            alert("Сообщение отправлено!");
            return true;
        };
    }
}

// Личный кабинет
function initAccount() {
    var saveBtn = document.querySelector('.btn-apply');
    if (saveBtn) {
        saveBtn.onclick = function () {
            alert("Изменения в профиле успешно сохранены!");
        };
    }

    var orderLinks = document.querySelectorAll('.order-link');
    for (var i = 0; i < orderLinks.length; i++) {
        orderLinks[i].onclick = function () {
            alert("Информация по заказу " + this.innerText + " подгружается...");
        };
    }
}

// Страница доставки
function initDelivery() {
    var downloadBtn = document.querySelector('.download-link');
    if (downloadBtn) {
        downloadBtn.onclick = function () {
            alert("Загрузка бланка заявления началась...");
        };
    }

    var blocks = document.querySelectorAll('.policy-block');
    for (var i = 0; i < blocks.length; i++) {
        blocks[i].onclick = function () {
            this.style.background = "#e9ecef";
        };
    }
}

// Отправка отзыва
function initReviewsPage() {
    var btn = document.getElementById('final-review-btn');
    var form = document.querySelector('.review-form');

    if (btn && form) {
        btn.onclick = function (e) {
            e.preventDefault();
            var name = form.querySelector('.review-name').value;
            var text = form.querySelector('.review-text').value;

            if (name.trim() === "" || text.trim() === "") {
                alert("Заполните имя и текст отзыва!");
            } else {
                alert("Спасибо, " + name + "! Ваш отзыв отправлен на модерацию.");
                form.reset();
            }
            return false;
        };
    }
}

// Админ-панель: Статистика
function initAdminDash() {
    var cards = document.querySelectorAll('.stat-card');
    for (var i = 0; i < cards.length; i++) {
        cards[i].onclick = function () { alert("Обновлено"); };
    }

    var items = document.querySelectorAll('.nav-item');
    for (var j = 0; j < items.length; j++) {
        items[j].onclick = function () {
            for (var k = 0; k < items.length; k++) items[k].style.background = "";
            this.style.background = "#2d3238";
        };
    }
}

// Админ-панель: Заказы
function initAdminOrders() {
    var btns = document.querySelectorAll('.btn-action');
    for (var i = 0; i < btns.length; i++) {
        btns[i].onclick = function () { alert("Действие выполнено!"); };
    }

    var badges = document.querySelectorAll('.order-badge');
    for (var j = 0; j < badges.length; j++) {
        badges[j].onclick = function () {
            this.innerText = "Готово";
            this.style.color = "green";
        };
    }
}

// Админ-панель: Настройки
function initAdminSettings() {
    var saveBtn = document.querySelector('.btn-primary');
    if (saveBtn) {
        saveBtn.onclick = function () { alert("Изменения сохранены!"); };
    }

    var dangerBtn = document.querySelector('.btn-danger-outline');
    if (dangerBtn) {
        dangerBtn.onclick = function () {
            if (confirm("Вы уверены? Это удалит все данные!")) {
                alert("База данных очищена (имитация)");
            }
        };
    }

    var toggle = document.querySelector('.switch input');
    if (toggle) {
        toggle.onchange = function () {
            alert(this.checked ? "2FA включена" : "2FA отключена");
        };
    }
}

// Админ-панель: Пользователи
function initAdminUsers() {
    var blockBtns = document.querySelectorAll('.btn-action.red');
    for (var i = 0; i < blockBtns.length; i++) {
        blockBtns[i].onclick = function () {
            if (confirm("Заблокировать пользователя?")) alert("Пользователь заблокирован");
        };
    }
}

// Админ-панель: Товары
function initAdminProducts() {
    var stocks = document.querySelectorAll('.badge-stock');
    for (var i = 0; i < stocks.length; i++) {
        stocks[i].onclick = function () {
            alert("На складе осталось: " + this.innerText);
        };
    }
}

// Точка входа
window.onload = function () {
    if (document.querySelector('.review-form')) initReviewsPage();

    if (!document.querySelector('.review-form')) {
        if (document.querySelector('.btn-apply') || document.querySelector('.order-link')) initAccount();
    }

    if (document.querySelector('.stats-grid')) initAdminDash();
    if (document.body.innerHTML.indexOf('Управление заказами') !== -1) initAdminOrders();
    if (document.querySelector('.settings-grid')) initAdminSettings();
    if (document.body.innerHTML.indexOf('Управление пользователями') !== -1) initAdminUsers();
    if (document.body.innerHTML.indexOf('Управление товарами') !== -1) initAdminProducts();
    if (document.querySelector('.contacts-grid')) initContacts();
    if (document.querySelector('.download-link')) initDelivery();

    initCart();
    initProductCard();
    initCatalog();

    console.log("TechZone scripts initialized.");
};