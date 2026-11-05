// Zvuky (Web Audio, syntetizované – žádné soubory) a vibrace.
window.fx = (() => {
    const read = (key) => { try { return localStorage.getItem(key) !== '0'; } catch { return true; } };
    const write = (key, v) => { try { localStorage.setItem(key, v ? '1' : '0'); } catch { } };

    let soundOn = read('sibenice.sound');
    let vibrationOn = read('sibenice.vibration');
    let ctx = null;

    const audio = () => {
        if (!ctx) {
            const AC = window.AudioContext || window.webkitAudioContext;
            if (!AC) return null;
            ctx = new AC();
        }
        if (ctx.state === 'suspended') ctx.resume();
        return ctx;
    };

    // Prohlížeč povolí zvuk až po prvním dotyku.
    window.addEventListener('pointerdown', () => { if (soundOn) audio(); }, { once: true });

    const tone = (freq, start, dur, type = 'sine', vol = 0.18, slideTo = null) => {
        const c = audio();
        if (!c) return;
        const t0 = c.currentTime + start;
        const osc = c.createOscillator();
        const gain = c.createGain();
        osc.type = type;
        osc.frequency.setValueAtTime(freq, t0);
        if (slideTo) osc.frequency.exponentialRampToValueAtTime(slideTo, t0 + dur);
        gain.gain.setValueAtTime(0.0001, t0);
        gain.gain.exponentialRampToValueAtTime(vol, t0 + 0.015);
        gain.gain.exponentialRampToValueAtTime(0.0001, t0 + dur);
        osc.connect(gain).connect(c.destination);
        osc.start(t0);
        osc.stop(t0 + dur + 0.02);
    };

    const sounds = {
        tap:  () => tone(520, 0, 0.05, 'triangle', 0.1),
        ok:   () => { tone(660, 0, 0.1, 'triangle'); tone(880, 0.09, 0.16, 'triangle'); },
        bad:  () => tone(240, 0, 0.28, 'sawtooth', 0.14, 120),
        hint: () => { tone(988, 0, 0.08, 'sine'); tone(1319, 0.08, 0.14, 'sine'); },
        win:  () => [523, 659, 784, 1047].forEach((f, i) => tone(f, i * 0.12, 0.22, 'triangle', 0.2)),
        lose: () => [392, 330, 262, 175].forEach((f, i) => tone(f, i * 0.2, 0.3, 'sawtooth', 0.13)),
    };

    const vibrations = {
        tap: [20],
        ok: [15],
        bad: [70],
        hint: [25, 40, 25],
        win: [40, 60, 40, 60, 140],
        lose: [320],
    };

    return {
        getSound: () => soundOn,
        getVibration: () => vibrationOn,
        setSound: (v) => { soundOn = v; write('sibenice.sound', v); },
        setVibration: (v) => { vibrationOn = v; write('sibenice.vibration', v); },
        play: (name) => {
            if (soundOn && sounds[name]) { try { sounds[name](); } catch { } }
            if (vibrationOn && vibrations[name] && navigator.vibrate) { try { navigator.vibrate(vibrations[name]); } catch { } }
        },
    };
})();
