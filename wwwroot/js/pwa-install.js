// Zachytí nativní instalační nabídku podporovaných prohlížečů (zejména Chrome).
window.pwaInstall = (() => {
    let deferredPrompt = null;
    let dotNetReference = null;

    const notify = available => {
        if (dotNetReference) {
            dotNetReference.invokeMethodAsync('SetInstallAvailable', available)
                .catch(() => { });
        }
    };

    window.addEventListener('beforeinstallprompt', event => {
        event.preventDefault();
        deferredPrompt = event;
        notify(true);
    });

    window.addEventListener('appinstalled', () => {
        deferredPrompt = null;
        notify(false);
    });

    return {
        init(reference) {
            dotNetReference = reference;
            notify(!!deferredPrompt);
        },
        async prompt() {
            if (!deferredPrompt) return;
            const promptEvent = deferredPrompt;
            deferredPrompt = null;
            await promptEvent.prompt();
            // Nabídka je jednorázová. Při zamítnutí ji Chrome může nabídnout znovu později.
            notify(false);
        }
    };
})();
