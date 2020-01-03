$(document).ready(function (t){
    var e, n, r = "".concat(["text", "password", "email", "url", "tel", "number", "search", "search-md"].map((function (t) {
        return "input[type=".concat(t, "]")
    })).join(", "), ", textarea");
    var o = function (t) {
        if (t.hasClass("validate")) {
            var e = t.val(),
                n = !e.length,
                r = !t[0].validity.badInput;
            if (n && r) t.removeClass("valid").removeClass("invalid");
            else {
                var i = t.is(":valid"),
                    o = Number(t.attr("length")) || 0;
                i && (!o || o > e.length) ? t.removeClass("invalid").addClass("valid") : t.removeClass("valid").addClass("invalid")
            }
        }
    };
    var i = function (t) {
        var e = t.siblings("label, i"),
            n = t.val().length,
            r = t.attr("placeholder");
        e["".concat(n || r ? "add" : "remove", "Class")]("active")
    };
    var a = function () {
        var e = t(void 0);
        if (e.val().length) {
            var n = t(".hiddendiv"),
                r = e.css("font-family"),
                i = e.css("font-size");
            i && n.css("font-size", i), r && n.css("font-family", r), "off" === e.attr("wrap") && n.css("overflow-wrap", "normal").css("white-space", "pre"), n.text("".concat(e.val(), "\n"));
            var o = n.html().replace(/\n/g, "<br>");
            n.html(o), n.css("width", e.is(":visible") ? e.width() : t(window).width() / 2), e.css("height", n.height())
        }
    };
    $(document).on("focus", r, (function (e) {
        $(e.target).siblings("label, i").addClass("active")
    }));
    $(document).ready(function () {
        $('input[value]').siblings("label, i").addClass("active")
        $('input[value=""]').siblings("label, i").removeClass("active")
    });
    $(document).on("blur", r, (function (e) {
        var n = $(e.target),
            r = !n.val(),
            i = !e.target.validity.badInput,
            a = void 0 === n.attr("placeholder");
        r && i && a && n.siblings("label, i").removeClass("active"), o(n)
    }));
    $(document).on("change", r, (function (e) {
        var n = $(e.target);
        i(n), o(n)
    }));
    $("input[autofocus]").siblings("label, i").addClass("active");
    $(document).on("reset", (function (e) {
        var n = $(e.target);
        n.is("form") && (n.find(r).removeClass("valid").removeClass("invalid").each((function (e, n) {
            var r = $(n),
                i = !r.val(),
                o = !r.attr("placeholder");
            i && o && r.siblings("label, i").removeClass("active")
        })), n.find("select.initialized").each((function (e, n) {
            var r = $(n),
                i = r.siblings("input.select-dropdown"),
                o = r.children("[selected]").val();
            r.val(o), i.val(o)
        })))
    }));

    (n = $(".md-textarea-auto")).length && (e = window.attachEvent ? function (t, e, n) {
        t.attachEvent("on".concat(e), n)
    } : function (t, e, n) {
        t.addEventListener(e, n, !1)
    }, n.each((function () {
        var t = this;

        function n() {
            t.style.height = "auto", t.style.height = "".concat(t.scrollHeight, "px")
        }

        function r() {
            window.setTimeout(n, 0)
        }
        e(t, "change", n), e(t, "cut", r), e(t, "paste", r), e(t, "drop", r), e(t, "keydown", r), n()
    })));
    var s = $("body");
    if (!$(".hiddendiv").first().length) {
        var l = $('<div class="hiddendiv common"></div>');
        s.append(l)
    }
    $(".materialize-textarea").each(a), s.on("keyup keydown", ".materialize-textarea", a); 
})