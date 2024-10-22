var TINY = {}; function T$(b) { return document.getElementById(b) } function T$$(b, g) { return g.getElementsByTagName(b) }
TINY.accordion = function () {
    function b(a) { this.n = a; this.h = []; this.c = [] } function g(a) { a.t = setInterval(function () { var e = a.offsetHeight; a.style.height = e + Math.ceil((1 == a.d ? a.m - e : e) / 10) * a.d + "px"; a.style.opacity = e / a.m; a.style.filter = "alpha(opacity=" + 100 * e / a.m + ")"; if (1 == a.d && e >= a.m || 1 != a.d && 1 == e) 1 == a.d && (a.style.height = "auto"), clearInterval(a.t) }, 10) } b.prototype.init = function (a, e, i, b, d) {
        var f = T$(a), a = x = 0; this.s = d || ""; w = []; n = f.childNodes; l = n.length; this.m = i || !1; for (a; a < l; a++) 3 != n[a].nodeType && (w[x] = n[a], x++);
        this.l = x; for (a = 0; a < this.l; a++) i = w[a], this.h[a] = h = T$$(e, i)[0], this.c[a] = c = T$$("div", i)[0], h.onclick = new Function(this.n + ".pr(false,this)"), b == a ? (h.className = this.s, c.style.height = "auto", c.d = 1) : (c.style.height = 0, c.d = -1)
    }; b.prototype.pr = function (a, e) {
        for (var b = 0; b < this.l; b++) {
            var j = this.h[b], d = this.c[b], f = d.style.height, f = "auto" == f ? 1 : parseInt(f); clearInterval(d.t); if (1 != f && -1 == d.d && (1 == a || j == e)) d.style.height = "", d.m = d.offsetHeight, d.style.height = f + "px", d.d = 1, j.className = this.s, g(d, 1); else if (0 < f && (-1 ==
a || this.m || j == e)) d.d = -1, j.className = "", g(d, -1)
        } 
    }; return { slider: b}
} ();