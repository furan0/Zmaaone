/* #################################################################
 * ####         Written by A.Delecroix & M.Garabello            ####
 * ####                for the Ludum Dare 45                    ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    /*******************************
     ***********TIMER INFO**********
     *******************************/
    [System.Serializable]
    public struct EventBeat { //Structure représentant un événement et le nombre de frame entre 2 appels
        public TimerEvent evenement;
        public int nbBeats;
        public int offset;
    }

    public List<EventBeat> events;
    private List<double> eventsTime = new List<double>();


    /*******************************
     ***********SONG INFO***********
     *******************************/
    
    // Song BPM
    public double bpm;

    // Song length
    public double songLength;

    // Song offset (seconds)
    public double offset;

    /*******************************
     *******POSITION TRACKING*******
     *******************************/

    // Music player
    public AudioSource player;

    // Song position
    double songPosSeconds;
    public double songPosBeats;

    // Duration of a beat (seconds)
    double beatDuration;

    // Absolute DSP time (seconds)
    double dspTime;

    // Absolute time when the song was started (seconds)
    double dspTimeStart;

    /*******************************
     ************METHODS************
     *******************************/

    // Use this for initialization
    void Start ()
    {
        // Update the duration of a beat
        beatDuration = 60.0 / bpm;
        dspTimeStart = AudioSettings.dspTime;

        for (int i = 0; i < events.Count; i++)
        {
            eventsTime.Add((double) events[i].offset + events[i].nbBeats);
        }
	}
   
    // Update is called once per frame
    void Update ()
    {
        if (player.isPlaying) // If the song is playing
        {
            // Update the actual song position
            dspTime = AudioSettings.dspTime;
            songPosSeconds = dspTime - dspTimeStart;
            songPosBeats = songPosSeconds / beatDuration;
        }

        //Gestion timer
        for (int i = 0; i < events.Count; i++)
        {
            if (eventsTime[i] <= songPosBeats) {
                events[i].evenement.raise();
                eventsTime[i] += events[i].nbBeats;
            }

        }

        if(songPosSeconds >= songLength) {
            StopPlaying();

            //Gestion timer
            for (int i = 0; i < events.Count; i++)
            {
                if (eventsTime[i] <= songPosBeats) {
                    events[i].evenement.raise();
                    eventsTime[i] += events[i].nbBeats;
                }

            }

            StartPlaying();
        }
	}

    // Used to know the state of the song playback
    public bool IsPlaying()
    {
        return player.isPlaying;
    }

    // Used to resume playing if the song isn't already playing
    public void StartPlaying()
    {
        if (!player.isPlaying)
        {
            // Update the duration of a beat
            beatDuration = 60.0 / bpm;


            // Update the position in the song file
            player.timeSamples = (int)(player.clip.frequency * (songPosSeconds + offset));

            // Set the absolute start time of the song
            dspTime = AudioSettings.dspTime;
            dspTimeStart = dspTime - songPosSeconds;

            for (int i = 0; i < events.Count; i++)
            {
                eventsTime[i] = (double) events[i].offset + events[i].nbBeats + songPosBeats;
            }

            player.Play(); // Start the song
        }
    }

    // Used to pause playing the song
    public void StopPlaying()
    {
        player.Pause(); // Pause the song
    }

    // Used to change the position in the song only if it's not playing
    public void SetPosition(double posBeats)
    {
        if (!player.isPlaying)
        {
            // Update the song position
            songPosBeats = posBeats;
            songPosSeconds = songPosBeats * beatDuration;
        }
    }

    // Used to get the position in the song (beats)
    public double GetPosition()
    {
        return songPosBeats;
    }

    // Used to get the length of the song (beats)
    public double GetLength()
    {
        return (player.clip.samples / player.clip.frequency) / beatDuration;
    }
}
