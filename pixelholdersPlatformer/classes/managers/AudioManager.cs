using static SDL2.SDL;
using static SDL2.SDL_mixer;

using System.Collections.Generic;

namespace pixelholdersPlatformer.classes.managers;

public class AudioManager
{

    private static AudioManager _instance;
    private Dictionary<string, IntPtr> _sounds;

    private bool _isRunning;

    private AudioManager()
    {
        _sounds = new Dictionary<string, IntPtr>();
        _isRunning = false;
        SDL_Init(SDL_INIT_AUDIO);
        Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 2048);
        // Mix_OpenAudio(44100, AUDIO_S16SYS, 2, 1024);
        LoadSounds();
    }

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AudioManager();
            }
            return _instance;
        }
    }

    private void LoadSounds()
    {
        // Load your sound files here
        _sounds["jump"] = Mix_LoadWAV("assets/SFX/jump.wav");
        _sounds["gameOver"] = Mix_LoadWAV("assets/SFX/gameOver.wav");
        _sounds["win"] = Mix_LoadWAV("assets/SFX/win.wav");
        _sounds["hit"] = Mix_LoadWAV("assets/SFX/hit.wav");
        _sounds["attack"] = Mix_LoadWAV("assets/SFX/attack.wav");
        _sounds["run"] = Mix_LoadWAV("assets/SFX/run.wav");
    }

    public void PlaySound(string soundName)
    {
        if (_sounds.ContainsKey(soundName))
        {
            Mix_PlayChannel(-1, _sounds[soundName], 0);
        }
    }

    public void StartRunning()
    {
        if (!_isRunning)
        {
            // start to play the sound on repeat
            Mix_PlayChannel(3, _sounds["run"], -1);
            _isRunning = true;
        }
    }

    public void StopRunning()
    {
        if (_isRunning)
        {
            // stop running sound
            Mix_HaltChannel(3);
            _isRunning = false;
        }
    }

    public void Dispose()
    {
        foreach (var sound in _sounds.Values)
        {
            Mix_FreeChunk(sound);
        }
        _sounds.Clear();
        Mix_CloseAudio();
        SDL_Quit();
    }
}