// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// ETİCARET — bütün sayfalarda çalışan küçük davranışlar.
// NOT: jQuery yerine tarayıcının kendi API'leri (querySelector, addEventListener, fetch) kullanıldı.
// jQuery sadece form doğrulama kütüphanesi (jquery-validation) için sayfada duruyor.

// Write your JavaScript code.
// Sağ üstte bildirim (toast) göster. Örnek: showToast('Kaydedildi', 'success')
function showToast(message, type = 'success') {
    const icons = {
        success: 'bi-check-circle-fill',
        danger: 'bi-x-circle-fill',
        warning: 'bi-exclamation-triangle-fill',
        info: 'bi-info-circle-fill'
    };

    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.setAttribute('role', 'alert');
    toast.innerHTML = `
        <div class="toast-body">
            <i class="bi ${icons[type] ?? icons.info}"></i>
            <span class="flex-grow-1"></span>
            <button type="button" class="btn-close ms-2" data-bs-dismiss="toast" aria-label="Kapat"></button>
        </div>`;
    toast.querySelector('span').textContent = message; // textContent: HTML olarak yorumlanmaz (güvenli)

    document.getElementById('toast-container').appendChild(toast);
    toast.addEventListener('hidden.bs.toast', () => toast.remove());
    new bootstrap.Toast(toast, { delay: 4500 }).show();
}

document.addEventListener('DOMContentLoaded', () => {

    // 1) Sunucunun TempData ile bıraktığı bildirim (_Toasts.cshtml)
    const serverNotify = document.getElementById('server-notify');
    if (serverNotify) showToast(serverNotify.dataset.text, serverNotify.dataset.type);

    // 2) Adet artır / azalt:  <div class="qty-stepper"> <button data-step="-1"> <input> <button data-step="1">
    document.querySelectorAll('.qty-stepper').forEach(stepper => {
        const input = stepper.querySelector('input');
        const autoSubmit = stepper.hasAttribute('data-autosubmit'); // Sepet sayfasında değişince form gönderilir

        stepper.querySelectorAll('[data-step]').forEach(button => button.addEventListener('click', () => {
            const min = Number(input.min || 1);
            const max = Number(input.max || 99);
            const value = Math.min(max, Math.max(min, Number(input.value) + Number(button.dataset.step)));
            if (value === Number(input.value)) return;

            input.value = value;
            if (autoSubmit) input.form.submit();
        }));

        if (autoSubmit) input.addEventListener('change', () => input.form.submit());
    });

    // 3) Ürün galerisi: küçük resme tıklayınca büyük resim değişir
    const mainImage = document.getElementById('gallery-main-image');
    document.querySelectorAll('.gallery-thumb').forEach(thumb => thumb.addEventListener('click', () => {
        mainImage.src = thumb.dataset.src;
        document.querySelectorAll('.gallery-thumb').forEach(t => t.classList.toggle('active', t === thumb));
    }));

    // 4) Şifreyi göster / gizle
    document.querySelectorAll('.toggle-password').forEach(button => button.addEventListener('click', () => {
        const input = button.parentElement.querySelector('input');
        const show = input.type === 'password';
        input.type = show ? 'text' : 'password';
        button.querySelector('i').className = show ? 'bi bi-eye-slash' : 'bi bi-eye';
    }));

    // 5) Silme gibi işlemlerden önce onay:  <form data-confirm="Silinsin mi?">
    document.querySelectorAll('form[data-confirm]').forEach(form => form.addEventListener('submit', event => {
        if (!confirm(form.dataset.confirm)) event.preventDefault();
    }));

    // 6) Seçim değişince formu gönder (sıralama, sipariş durumu):  <select data-autosubmit>
    document.querySelectorAll('select[data-autosubmit]').forEach(select =>
        select.addEventListener('change', () => select.form.submit()));

    // 7) Yönetim paneli: yüklenecek resimlerin önizlemesi  <input type="file" data-preview="#hedef">
    document.querySelectorAll('input[type=file][data-preview]').forEach(input => input.addEventListener('change', () => {
        const target = document.querySelector(input.dataset.preview);
        target.innerHTML = '';
        [...input.files].forEach(file => {
            const tile = document.createElement('div');
            tile.className = 'image-tile';
            const image = document.createElement('img');
            image.src = URL.createObjectURL(file);
            tile.appendChild(image);
            target.appendChild(tile);
        });
    }));

    // 8) Yönetim paneli: silinecek resmi işaretleyince soluklaştır
    document.querySelectorAll('.image-tile input[type=checkbox]').forEach(checkbox =>
        checkbox.addEventListener('change', () =>
            checkbox.closest('.image-tile').classList.toggle('is-removed', checkbox.checked)));
});