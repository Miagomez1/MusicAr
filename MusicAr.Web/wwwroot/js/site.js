document.addEventListener('DOMContentLoaded', () => {
    const audio = document.getElementById('audio');
    if (!audio) return;

    const playSvg = "<path d='M8 5v14l11-7z'/>";
    const pauseSvg = "<path d='M6 4h4v16H6zM14 4h4v16h-4z'/>";

    const miniTitulo = document.getElementById('player-title');
    const miniArtista = document.getElementById('player-artist');
    const miniCover = document.getElementById('player-cover');
    const miniPlayBtn = document.getElementById('btn-play-pause');
    const miniProgressContainer = document.getElementById('progress-bar-container');
    const miniProgressBar = document.getElementById('progress-bar');
    const btnPlayerPrev = document.getElementById('btnPlayerPrev');
    const btnPlayerNext = document.getElementById('btnPlayerNext');

    const expTitulo = document.getElementById('expanded-title');
    const expArtista = document.getElementById('expanded-artist');
    const expCover = document.getElementById('expanded-cover');
    const expCoverPlaceholder = document.getElementById('playerExpCoverPlaceholder');
    const expPlayBtn = document.getElementById('expanded-btn-play-pause');
    const expProgressContainer = document.getElementById('expanded-progress-container');
    const expProgressBar = document.getElementById('expanded-progress-bar');
    const currentTimeEl = document.getElementById('current-time');
    const totalDurationEl = document.getElementById('total-duration');
    const btnExpPrev = document.getElementById('btnExpPrev');
    const btnExpNext = document.getElementById('btnExpNext');
    const btnPlayerFavorito = document.getElementById('btnPlayerFavorito');

    let currentButton = null;
    let currentTrackId = null;
    const favoritosSet = new Set();

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
            miniCover.style.backgroundImage = coverUrl ? `url('${coverUrl}')` : '';
        }

        if (expTitulo) expTitulo.textContent = titulo;
        if (expArtista) expArtista.textContent = artista;
        if (expCover) {
            if (coverUrl) {
                expCover.style.backgroundImage = `url('${coverUrl}')`;
                if (expCoverPlaceholder) expCoverPlaceholder.style.display = 'none';
            } else {
                expCover.style.backgroundImage = '';
                if (expCoverPlaceholder) expCoverPlaceholder.style.display = 'block';
            }
        }

        const colaTit = document.getElementById('colaActualTitulo');
        const colaArt = document.getElementById('colaActualArtista');
        if (colaTit) colaTit.textContent = titulo;
        if (colaArt) colaArt.textContent = artista;

        const colaCover = document.getElementById('colaActualCover');
        if (colaCover) {
            colaCover.style.backgroundImage = coverUrl ? `url('${coverUrl}')` : '';
        }
    }

    function actualizarFavorito(cancionId, esFavorito) {
        if (esFavorito) {
            favoritosSet.add(String(cancionId));
        } else {
            favoritosSet.delete(String(cancionId));
        }

        document.querySelectorAll(`.btn-favorito[data-cancion-id="${cancionId}"]`).forEach(btn => {
            if (esFavorito) {
                btn.classList.add('activo');
            } else {
                btn.classList.remove('activo');
            }
        });

        if (btnPlayerFavorito && String(currentTrackId) === String(cancionId)) {
            if (esFavorito) {
                btnPlayerFavorito.classList.add('activo');
            } else {
                btnPlayerFavorito.classList.remove('activo');
            }
        }
    }

    async function toggleFavorito(cancionId) {
        if (document.body.dataset.autenticado !== 'true') {
            window.location.href = '/Cuenta/Login?returnUrl=' + encodeURIComponent(window.location.pathname + window.location.search);
            return;
        }

        if (!cancionId) return;

        try {
            const res = await fetch('/Favoritos/Toggle/' + cancionId, {
                method: 'POST',
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });
            if (res.ok) {
                const data = await res.json();
                actualizarFavorito(cancionId, data.esFavorito);
            }
        } catch (err) {
            console.warn(err);
        }
    }

    if (btnPlayerFavorito) {
        btnPlayerFavorito.addEventListener('click', () => {
            if (currentTrackId) {
                toggleFavorito(currentTrackId);
            }
        });
    }

    if (document.body.dataset.autenticado === 'true') {
        fetch('/Favoritos/Ids')
            .then(res => res.json())
            .then(ids => {
                if (Array.isArray(ids)) {
                    ids.forEach(id => {
                        favoritosSet.add(String(id));
                        document.querySelectorAll(`.btn-favorito[data-cancion-id="${id}"]`).forEach(btn => {
                            btn.classList.add('activo');
                        });
                    });
                }
            })
            .catch(err => console.warn(err));
    }

    function playTrack(btn) {
        if (document.body.dataset.autenticado !== 'true') {
            window.location.href = '/Cuenta/Login?returnUrl=' + encodeURIComponent(window.location.pathname + window.location.search);
            return;
        }

        const fila = btn.closest('.fila') || btn;
        const audioUrl = btn.dataset.audio || fila.dataset.audioUrl || btn.dataset.audioUrl;
        const titulo = btn.dataset.titulo || fila.dataset.title || btn.dataset.title || 'Canción';
        const artista = btn.dataset.artista || fila.dataset.artist || btn.dataset.artist || 'Artista';
        const cover = btn.dataset.cover || fila.dataset.cover || '';
        const cancionId = btn.dataset.cancionId || btn.dataset.id || fila.dataset.id;

        if (!audioUrl) return;

        if (cancionId) {
            currentTrackId = cancionId;
            if (btnPlayerFavorito) {
                if (favoritosSet.has(String(cancionId))) {
                    btnPlayerFavorito.classList.add('activo');
                } else {
                    btnPlayerFavorito.classList.remove('activo');
                }
            }
        }

        const tempAnchor = document.createElement('a');
        tempAnchor.href = audioUrl;
        const fullAudioUrl = tempAnchor.href;

        if (audio.src === fullAudioUrl) {
            if (audio.paused) {
                audio.play();
            } else {
                audio.pause();
            }
            return;
        }

        if (currentButton && currentButton !== btn) {
            setIcon(currentButton, false);
            const filaVieja = currentButton.closest('.fila');
            if (filaVieja) filaVieja.classList.remove('sonando');
        }

        currentButton = btn;
        const filaActual = btn.closest('.fila');
        if (filaActual) filaActual.classList.add('sonando');

        audio.src = fullAudioUrl;
        audio.play().catch(e => console.warn(e));

        updateTrackInfo(titulo, artista, cover);
        updateQueue();
    }

    document.addEventListener('click', (e) => {
        const btnFav = e.target.closest('.btn-favorito');
        if (btnFav) {
            e.preventDefault();
            e.stopPropagation();
            const id = btnFav.dataset.cancionId;
            toggleFavorito(id);
            return;
        }

        const btnPlay = e.target.closest('.btn-play');
        if (btnPlay) {
            e.preventDefault();
            playTrack(btnPlay);
            return;
        }

        const songRow = e.target.closest('.song-row, .fila');
        if (songRow && !e.target.closest('button')) {
            const playInRow = songRow.querySelector('.btn-play') || songRow;
            playTrack(playInRow);
        }
    });

    if (miniPlayBtn) {
        miniPlayBtn.addEventListener('click', () => {
            if (document.body.dataset.autenticado !== 'true') {
                window.location.href = '/Cuenta/Login?returnUrl=' + encodeURIComponent(window.location.pathname + window.location.search);
                return;
            }
            if (!audio.src) return;
            if (audio.paused) audio.play(); else audio.pause();
        });
    }

    if (expPlayBtn) {
        expPlayBtn.addEventListener('click', () => {
            if (document.body.dataset.autenticado !== 'true') {
                window.location.href = '/Cuenta/Login?returnUrl=' + encodeURIComponent(window.location.pathname + window.location.search);
                return;
            }
            if (!audio.src) return;
            if (audio.paused) audio.play(); else audio.pause();
        });
    }

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
        if (miniProgressBar) miniProgressBar.style.width = `${pct}%`;
        if (expProgressBar) expProgressBar.style.width = `${pct}%`;
        if (currentTimeEl) currentTimeEl.textContent = formatTime(audio.currentTime);
        if (totalDurationEl) totalDurationEl.textContent = formatTime(audio.duration);
    });

    function updateQueue() {
        const queueContainer = document.getElementById('colaProximasCanciones');
        if (!queueContainer) return;

        queueContainer.innerHTML = '';
        const allSongButtons = Array.from(document.querySelectorAll('.fila .btn-play, .song-row .btn-play'));
        if (!allSongButtons.length || !currentButton) return;

        const currentIndex = allSongButtons.indexOf(currentButton);
        if (currentIndex === -1) return;

        for (let i = 1; i <= 4; i++) {
            const nextIdx = (currentIndex + i) % allSongButtons.length;
            if (nextIdx === currentIndex) break;

            const btn = allSongButtons[nextIdx];
            const fila = btn.closest('.fila, .song-row') || btn;
            const titulo = btn.dataset.titulo || fila.dataset.title || 'Canción';
            const artista = btn.dataset.artista || fila.dataset.artist || 'Artista';
            const cover = btn.dataset.cover || fila.dataset.cover || '';
            const id = btn.dataset.cancionId || btn.dataset.id || fila.dataset.id;

            const item = document.createElement('div');
            item.className = 'cola-item';
            item.style.cursor = 'pointer';
            item.innerHTML = `
                <span class="cover cover-md" style="background-image: ${cover ? `url('${cover}')` : 'none'}; background-size: cover; background-position: center;"></span>
                <div class="text-truncate">
                    <div class="fw-semibold text-truncate">${titulo}</div>
                    <small class="text-body-secondary text-truncate">${artista}</small>
                </div>
            `;
            item.addEventListener('click', () => {
                playTrack(btn);
            });
            queueContainer.appendChild(item);
        }
    }

    function playNext() {
        const buttons = Array.from(document.querySelectorAll('.fila .btn-play, .song-row .btn-play'));
        if (!buttons.length) return;
        const idx = buttons.indexOf(currentButton);
        const nextIdx = (idx + 1) % buttons.length;
        playTrack(buttons[nextIdx]);
    }

    function playPrev() {
        const buttons = Array.from(document.querySelectorAll('.fila .btn-play, .song-row .btn-play'));
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

    if (miniProgressContainer) miniProgressContainer.addEventListener('click', (e) => seek(e, miniProgressContainer));
    if (expProgressContainer) expProgressContainer.addEventListener('click', (e) => seek(e, expProgressContainer));
});
