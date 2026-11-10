// Spojení dvou mobilů přes veřejnou službu ntfy.sh (publish/subscribe, bez vlastního serveru).
// Každá místnost = jedno "téma" (topic). Odesílá se obyčejný POST, příjem běží přes SSE (EventSource).
window.net = (() => {
    const base = 'https://ntfy.sh/';
    let es = null;
    let ref = null;

    const close = () => {
        if (es) { es.close(); es = null; }
        ref = null;
    };

    return {
        connect: (topic, dotnetRef) => {
            close();
            ref = dotnetRef;
            es = new EventSource(base + encodeURIComponent(topic) + '/sse');
            es.onopen = () => ref && ref.invokeMethodAsync('OnOpen').catch(() => { });
            es.onerror = () => ref && ref.invokeMethodAsync('OnError').catch(() => { });
            es.onmessage = e => {
                try {
                    const d = JSON.parse(e.data);
                    if (d.event === 'message' && ref) ref.invokeMethodAsync('OnMessage', d.message).catch(() => { });
                } catch { }
            };
        },

        send: async (topic, text) => {
            try {
                const r = await fetch(base + encodeURIComponent(topic), { method: 'POST', body: text });
                return r.ok;
            } catch { return false; }
        },

        close,

        share: async (title, text, url) => {
            try {
                if (navigator.share) { await navigator.share({ title, text, url }); return 'shared'; }
                await navigator.clipboard.writeText(url);
                return 'copied';
            } catch { return 'failed'; }
        },

        getStr: (key) => { try { return localStorage.getItem(key) || ''; } catch { return ''; } },
        setStr: (key, v) => { try { localStorage.setItem(key, v); } catch { } },
    };
})();
