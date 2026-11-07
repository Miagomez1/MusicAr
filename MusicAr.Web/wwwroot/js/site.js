document.addEventListener('DOMContentLoaded', () => {
    const audio = document.getElementById('audio');
    if (!audio) return;

    const playSvg = "<path d='M8 5v14l11-7z'/>";
    const pauseSvg = "<path d='M6 4h4v16H6zM14 4h4v16h-4z'/>";

    // Elementos Mini
    const miniTitulo = document.getElementById('playerMiniTitulo');
    const miniArtista = document.getElementById('playerMiniArtista');
    const miniCover = document.getElementById('playerMiniCover');
    const miniPlayBtn = document.getElementById('btnPlayerPlay');
    const miniProgressBar = document.getElementById('playerProgressBar');
    const miniProgressFill = document.getElementById('playerProgressFill');
    const btnPlayerPrev = document.getElementById('btnPlayerPrev');
    const btnPlayerNext = document.getElementById('btnPlayerNext');

    // Elementos Expandido
    const expTitulo = document.getElementById('playerExpTitulo');
    const expArtista = document.getElementById('playerExpArtista');
    const expCover = document.getElementById('playerExpCover');
    const expCoverPlaceholder = document.getElementById('playerExpCoverPlaceholder');
    const expPlayBtn = document.getElementById('btnExpPlay');
    const expProgressBar = document.getElementById('playerExpProgressBar');
    const expProgressFill = document.getElementById('playerExpProgressFill');
    const expCurrentTime = document.getElementById('playerExpCurrentTime');
    const expTotalTime = document.getElementById('playerExpTotalTime');
    const btnExpPrev = document.getElementById('btnExpPrev');
    const btnExpNext = document.getElementById('btnExpNext');

    let currentButton = null;

    function formatTime(seconds) {
        if (isNaN(seconds) || seconds < 0) return "0:00";
        const mins = Math.floor(seconds / 60);
        const secs = Math.floor(seconds % 60);
        return `${mins}:${secs < 10 ? '0' : ''}${secs}`;
    }

    function setIcon(btn, isPlaying) {
        if (!btn) return;
        const svg = btn.querySelector('svg');
        if (svg) {
            svg.innerHTML = isPlaying ? pauseSvg : playSvg;
        }
        btn.setAttribute('aria-label', isPlaying ? 'Pausar' : 'Reproducir');
    }

    function updateTrackInfo(titulo, artista, coverUrl) {
        if (miniTitulo) miniTitulo.textContent = titulo;
        if (miniArtista) miniArtista.textContent = artista;
        if (miniCover) {
            if (coverUrl) {
                miniCover.src = coverUrl;
                miniCover.style.display = 'block';
            } else {
                miniCover.style.display = 'none';
            }
        }

        if (expTitulo) expTitulo.textContent = titulo;
        if (expArtista) expArtista.textContent = artista;
        if (expCover) {
            if (coverUrl) {
                expCover.src = coverUrl;
                expCover.style.display = 'block';
                if (expCoverPlaceholder) expCoverPlaceholder.style.display = 'none';
            } else {
                expCover.style.display = 'none';
                if (expCoverPlaceholder) expCoverPlaceholder.style.display = 'block';
            }
        }

        const colaTit = document.getElementById('colaActualTitulo');
        const colaArt = document.getElementById('colaActualArtista');
        if (colaTit) colaTit.textContent = titulo;
        if (colaArt) colaArt.textContent = artista;
    }

    function playTrack(btn) {
        const audioUrl = btn.dataset.audio;
        const titulo = btn.dataset.titulo || 'Canción';
        const artista = btn.dataset.artista || 'Artista';
        const cover = btn.dataset.cover || '';

        if (!audioUrl) return;

        // Si es la misma canción ya cargada
        if (audio.src === audioUrl) {
            if (audio.paused) {
                audio.play();
            } else {
                audio.pause();
            }
            return;
        }

        // Nueva canción seleccionada
        if (currentButton && currentButton !== btn) {
            setIcon(currentButton, false);
            const filaVieja = currentButton.closest('.fila');
            if (filaVieja) filaVieja.classList.remove('sonando');
        }

        currentButton = btn;
        const filaActual = btn.closest('.fila');
        if (filaActual) filaActual.classList.add('sonando');

        audio.src = audioUrl;
        audio.play().catch(e => console.warn("Error al reproducir audio:", e));

        updateTrackInfo(titulo, artista, cover);
    }

    // Clic en el botón Play de la fila
    document.addEventListener('click', (e) => {
        const btn = e.target.closest('.fila .btn-play');
        if (btn) {
            e.preventDefault();
            playTrack(btn);
        }
    });

    // Control Play/Pausa en la barra inferior
    if (miniPlayBtn) {
        miniPlayBtn.addEventListener('click', () => {
            if (!audio.src) return;
            if (audio.paused) audio.play(); else audio.pause();
        });
    }

    // Control Play/Pausa en el modal expandido
    if (expPlayBtn) {
        expPlayBtn.addEventListener('click', () => {
            if (!audio.src) return;
            if (audio.paused) audio.play(); else audio.pause();
        });
    }

    // Eventos nativos del elemento <audio>
    audio.addEventListener('play', () => {
        setIcon(currentButton, true);
        setIcon(miniPlayBtn, true);
        setIcon(expPlayBtn, true);
    });

    audio.addEventListener('pause', () => {
        setIcon(currentButton, false);
        setIcon(miniPlayBtn, false);
        setIcon(expPlayBtn, false);
    });

    audio.addEventListener('timeupdate', () => {
        if (!audio.duration) return;
        const pct = (audio.currentTime / audio.duration) * 100;
        if (miniProgressFill) miniProgressFill.style.width = `${pct}%`;
        if (expProgressFill) expProgressFill.style.width = `${pct}%`;
        if (expCurrentTime) expCurrentTime.textContent = formatTime(audio.currentTime);
        if (expTotalTime) expTotalTime.textContent = formatTime(audio.duration);
    });

    function playNext() {
        const buttons = Array.from(document.querySelectorAll('.fila .btn-play'));
        if (!buttons.length) return;
        const idx = buttons.indexOf(currentButton);
        const nextIdx = (idx + 1) % buttons.length;
        playTrack(buttons[nextIdx]);
    }

    function playPrev() {
        const buttons = Array.from(document.querySelectorAll('.fila .btn-play'));
        if (!buttons.length) return;
        const idx = buttons.indexOf(currentButton);
        const prevIdx = (idx - 1 + buttons.length) % buttons.length;
        playTrack(buttons[prevIdx]);
    }

    audio.addEventListener('ended', playNext);

    if (btnPlayerNext) btnPlayerNext.addEventListener('click', playNext);
    if (btnExpNext) btnExpNext.addEventListener('click', playNext);
    if (btnPlayerPrev) btnPlayerPrev.addEventListener('click', playPrev);
    if (btnExpPrev) btnExpPrev.addEventListener('click', playPrev);

    function seek(e, bar) {
        if (!audio.duration) return;
        const rect = bar.getBoundingClientRect();
        const clickX = e.clientX - rect.left;
        audio.currentTime = (clickX / rect.width) * audio.duration;
    }

    if (miniProgressBar) miniProgressBar.addEventListener('click', (e) => seek(e, miniProgressBar));
    if (expProgressBar) expProgressBar.addEventListener('click', (e) => seek(e, expProgressBar));
});