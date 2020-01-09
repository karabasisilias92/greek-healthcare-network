function main() {
    var title = $('title').text();
    var li = $('ul.nav-menu > li');
    if (title.includes('Home')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('HOME')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Doctors')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('DOCTORS')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Book Appointment')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('BOOK APPOINTMENT')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Appointments') && !title.includes('Appointments History') ) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('APPOINTMENTS')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
}
window.addEventListener('load', main);