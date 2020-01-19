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
    else if (title.includes('Insureds')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('INSUREDS')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Visitor Messages')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('VISITOR MESSAGES')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Messages')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('MESSAGES')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Insured Plans')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('INSURED PLANS')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Doctor Plans')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('DOCTOR PLANS')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Appointment Cost Per Specialty')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('APPOINTMENT COST PER SPECIALTY')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Appointments Report Page')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('APPOINTMENTS REPORT PAGE')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Subscriptions Expired Report Page')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('SUBSCRIPTIONS EXPIRED REPORT PAGE')) {
                li[i].classList.add('menu-active');
                break;
            }
        }
    }
    else if (title.includes('Create Admin User')) {
        for (let i = 0; i < li.length; i++) {
            if (li[i].innerText.includes('CREATE ADMIN USER')) {
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