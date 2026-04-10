/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Interface Segregation Principle - Real-World
 * FILE      : 03_ISP_RealWorld.cs
 * PURPOSE   : Demonstrates real-world ISP applications including
 *             worker management, document processing, and
 *             game character systems
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._04_InterfaceSegregation._03_ISP_RealWorld
{
    /// <summary>
    /// Demonstrates real-world Interface Segregation Principle
    /// </summary>
    public class ISPRealWorldDemo
    {
        /// <summary>
        /// Entry point for ISP real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Worker Management with ISP
            // ═══════════════════════════════════════════════════════════
            // Workers implement only the interfaces they need
            // No forced implementation of unused capabilities

            Console.WriteLine("=== ISP Real-World ===\n");

            // Output: --- Worker Management ---
            Console.WriteLine("--- Worker Management ---");

            // Manager can manage but not code
            IManaged manager = new ProjectManager();
            manager.AssignTask("Design UI");
            manager.ReviewWork("Design complete");
            // Output: Manager assigned task: Design UI
            // Output: Manager reviewed work: Design complete

            // Developer can code but not manage
            ICanCode developer = new SoftwareDeveloper();
            developer.WriteCode("Feature X");
            developer.ReviewCode("Code reviewed");
            // Output: Developer wrote code: Feature X
            // Output: Developer reviewed code: Code reviewed

            // Lead does both
            IManaged leadDev = new TechLead();
            leadDev.AssignTask("Write API");
            leadDev.ReviewWork("API complete");
            // Output: TechLead assigned task: Write API
            // Output: TechLead reviewed work: API complete

            ICanCode leadDevCode = (ICanCode)leadDev;
            leadDevCode.WriteCode("API endpoint");
            // Output: TechLead wrote code: API endpoint

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Document Processing with ISP
            // ═══════════════════════════════════════════════════════════
            // Document handlers implement only needed interfaces
            // Each processor has specific, focused responsibility

            // Output: --- Document Processing ---
            Console.WriteLine("\n--- Document Processing ---");

            var processors = new List<IDocumentProcessor>
            {
                new PdfProcessor(),
                new WordProcessor(),
                new ExcelProcessor()
            };

            foreach (var processor in processors)
            {
                processor.Open();
                processor.Process();
                processor.Save();
                // Output: PDF opened
                // Output: PDF processed
                // Output: PDF saved
                // Output: Word opened
                // Output: Word processed
                // Output: Word saved
                // Output: Excel opened
                // Output: Excel processed
                // Output: Excel saved
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Game Character System with ISP
            // ═══════════════════════════════════════════════════════════
            // Character capabilities are separated into interfaces
            // Characters implement only what they can do

            // Output: --- Game Character System ---
            Console.WriteLine("\n--- Game Character System ---");

            // Warrior attacks and defends
            ICanAttack warrior = new Warrior();
            warrior.Attack("Slash");
            // Output: Warrior attacks: Slash
            ICanDefend warriorDef = new Warrior();
            warriorDef.Defend("Block");
            // Output: Warrior defends: Block

            // Mage can cast spells
            ICanCastSpells mage = new Mage();
            mage.CastSpell("Fireball");
            // Output: Mage casts: Fireball

            // Ranger can move and track
            ICanMove ranger = new Ranger();
            ranger.Move("Sneak");
            // Output: Ranger moves: Sneak
            ICanTrack target = new Ranger();
            target.Track("Enemy");
            // Output: Ranger tracks: Enemy

            // Paladin does everything
            ICanAttack paladin = new Paladin();
            paladin.Attack("Smite");
            // Output: Paladin attacks: Smite
            ICanDefend paladinDef = new Paladin();
            paladinDef.Defend("Shield");
            // Output: Paladin defends: Shield
            ICanCastSpells paladinSpell = new Paladin();
            paladinSpell.CastSpell("Heal");
            // Output: Paladin casts: Heal

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Media Player with ISP
            // ═══════════════════════════════════════════════════════════
            // Media players implement only supported features

            // Output: --- Media Player ---
            Console.WriteLine("\n--- Media Player ---");

            // Basic player only plays audio
            IPlayAudio basicPlayer = new BasicMediaPlayer();
            basicPlayer.PlayAudio("song.mp3");
            // Output: Playing audio: song.mp3

            // Video player plays both
            IPlayAudio videoAudio = new VideoMediaPlayer();
            videoAudio.PlayAudio("video.mp4");
            // Output: Playing audio: video.mp4
            IPlayVideo videoPlayer = new VideoMediaPlayer();
            videoPlayer.PlayVideo("video.mp4");
            // Output: Playing video: video.mp4

            // Streaming player can also stream
            IPlayAudio streamAudio = new StreamingMediaPlayer();
            streamAudio.PlayAudio("stream");
            // Output: Playing audio: stream
            IPlayVideo streamVideo = new StreamingMediaPlayer();
            streamVideo.PlayVideo("stream");
            // Output: Playing video: stream
            ICanStream stream = new StreamingMediaPlayer();
            stream.StartStream("live");
            // Output: Starting stream: live

            Console.WriteLine("\n=== ISP Real-World Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 1: Worker Management Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Worker that can be managed - small, focused interface
    /// </summary>
    public interface IManaged
    {
        void AssignTask(string task);
        void ReviewWork(string work);
    }

    /// <summary>
    /// Worker that can code - small, focused interface
    /// </summary>
    public interface ICanCode
    {
        void WriteCode(string feature);
        void ReviewCode(string code);
    }

    /// <summary>
    /// Project manager - implements IManaged
    /// </summary>
    public class ProjectManager : IManaged
    {
        public void AssignTask(string task)
        {
            Console.WriteLine($"   Manager assigned task: {task}");
        }

        public void ReviewWork(string work)
        {
            Console.WriteLine($"   Manager reviewed work: {work}");
        }
    }

    /// <summary>
    /// Software developer - implements ICanCode
    /// </summary>
    public class SoftwareDeveloper : ICanCode
    {
        public void WriteCode(string feature)
        {
            Console.WriteLine($"   Developer wrote code: {feature}");
        }

        public void ReviewCode(string code)
        {
            Console.WriteLine($"   Developer reviewed code: {code}");
        }
    }

    /// <summary>
    /// Tech lead - implements both interfaces
    /// </summary>
    public class TechLead : IManaged, ICanCode
    {
        public void AssignTask(string task)
        {
            Console.WriteLine($"   TechLead assigned task: {task}");
        }

        public void ReviewWork(string work)
        {
            Console.WriteLine($"   TechLead reviewed work: {work}");
        }

        public void WriteCode(string feature)
        {
            Console.WriteLine($"   TechLead wrote code: {feature}");
        }

        public void ReviewCode(string code)
        {
            Console.WriteLine($"   TechLead reviewed code: {code}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 2: Document Processing Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Document processor interface - opened, processed, saved
    /// </summary>
    public interface IDocumentProcessor
    {
        void Open();
        void Process();
        void Save();
    }

    /// <summary>
    /// PDF processor - implements all three methods
    /// </summary>
    public class PdfProcessor : IDocumentProcessor
    {
        public void Open() => Console.WriteLine("   PDF opened");
        public void Process() => Console.WriteLine("   PDF processed");
        public void Save() => Console.WriteLine("   PDF saved");
    }

    /// <summary>
    /// Word processor - implements all three methods
    /// </summary>
    public class WordProcessor : IDocumentProcessor
    {
        public void Open() => Console.WriteLine("   Word opened");
        public void Process() => Console.WriteLine("   Word processed");
        public void Save() => Console.WriteLine("   Word saved");
    }

    /// <summary>
    /// Excel processor - implements all three methods
    /// </summary>
    public class ExcelProcessor : IDocumentProcessor
    {
        public void Open() => Console.WriteLine("   Excel opened");
        public void Process() => Console.WriteLine("   Excel processed");
        public void Save() => Console.WriteLine("   Excel saved");
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 3: Game Character Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Character can attack
    /// </summary>
    public interface ICanAttack
    {
        void Attack(string attackType);
    }

    /// <summary>
    /// Character can defend
    /// </summary>
    public interface ICanDefend
    {
        void Defend(string defenseType);
    }

    /// <summary>
    /// Character can cast spells
    /// </summary>
    public interface ICanCastSpells
    {
        void CastSpell(string spell);
    }

    /// <summary>
    /// Character can move
    /// </summary>
    public interface ICanMove
    {
        void Move(string movementType);
    }

    /// <summary>
    /// Character can track targets
    /// </summary>
    public interface ICanTrack
    {
        void Track(string target);
    }

    /// <summary>
    /// Warrior - attack and defend
    /// </summary>
    public class Warrior : ICanAttack, ICanDefend
    {
        public void Attack(string attackType) => Console.WriteLine($"   Warrior attacks: {attackType}");
        public void Defend(string defenseType) => Console.WriteLine($"   Warrior defends: {defenseType}");
    }

    /// <summary>
    /// Mage - cast spells only
    /// </summary>
    public class Mage : ICanCastSpells
    {
        public void CastSpell(string spell) => Console.WriteLine($"   Mage casts: {spell}");
    }

    /// <summary>
    /// Ranger - move and track
    /// </summary>
    public class Ranger : ICanMove, ICanTrack
    {
        public void Move(string movementType) => Console.WriteLine($"   Ranger moves: {movementType}");
        public void Track(string target) => Console.WriteLine($"   Ranger tracks: {target}");
    }

    /// <summary>
    /// Paladin - all capabilities
    /// </summary>
    public class Paladin : ICanAttack, ICanDefend, ICanCastSpells
    {
        public void Attack(string attackType) => Console.WriteLine($"   Paladin attacks: {attackType}");
        public void Defend(string defenseType) => Console.WriteLine($"   Paladin defends: {defenseType}");
        public void CastSpell(string spell) => Console.WriteLine($"   Paladin casts: {spell}");
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 4: Media Player Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Can play audio
    /// </summary>
    public interface IPlayAudio
    {
        void PlayAudio(string file);
    }

    /// <summary>
    /// Can play video
    /// </summary>
    public interface IPlayVideo
    {
        void PlayVideo(string file);
    }

    /// <summary>
    /// Can stream content
    /// </summary>
    public interface ICanStream
    {
        void StartStream(string streamId);
    }

    /// <summary>
    /// Basic media player - audio only
    /// </summary>
    public class BasicMediaPlayer : IPlayAudio
    {
        public void PlayAudio(string file) => Console.WriteLine($"   Playing audio: {file}");
    }

    /// <summary>
    /// Video player - audio and video
    /// </summary>
    public class VideoMediaPlayer : IPlayAudio, IPlayVideo
    {
        public void PlayAudio(string file) => Console.WriteLine($"   Playing audio: {file}");
        public void PlayVideo(string file) => Console.WriteLine($"   Playing video: {file}");
    }

    /// <summary>
    /// Streaming player - all capabilities
    /// </summary>
    public class StreamingMediaPlayer : IPlayAudio, IPlayVideo, ICanStream
    {
        public void PlayAudio(string file) => Console.WriteLine($"   Playing audio: {file}");
        public void PlayVideo(string file) => Console.WriteLine($"   Playing video: {file}");
        public void StartStream(string streamId) => Console.WriteLine($"   Starting stream: {streamId}");
    }
}
