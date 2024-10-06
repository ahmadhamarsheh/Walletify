function toggleNav() {
    var sidemenu = document.getElementById("sidemenu");
    var overlay = document.getElementById("overlay");

    // التحقق من حالة القائمة الجانبية (مفتوحة أو مغلقة)
    if (sidemenu.classList.contains("open")) {
        sidemenu.classList.remove("open");
        overlay.style.display = "none"; // إخفاء overlay
    } else {
        sidemenu.classList.add("open");
        overlay.style.display = "block"; // عرض overlay فقط عند فتح القائمة
    }
}

// إغلاق القائمة عند الضغط على الـ overlay
document.getElementById("overlay").addEventListener("click", function () {
    toggleNav(); // استدعاء toggleNav لإغلاق القائمة
});

