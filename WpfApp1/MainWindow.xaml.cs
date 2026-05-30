using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CyberSecurity_Bot
{
    /// <summary>
    /// BIMO — Cybersecurity Awareness Bot  |  Part 2
    /// WPF GUI with: Keyword Recognition, Random Responses, Conversation Flow,
    /// Memory & Recall, Sentiment Detection, Error Handling, Code Optimisation.
    /// </summary>
    public partial class MainWindow : Window
    {
        // ═══════════════════════════════════════════════════════════
        //  DATA LAYER  (Code Optimisation — dictionaries / lists)
        // ═══════════════════════════════════════════════════════════

        /// <summary>Multiple random responses per keyword (Random Responses requirement).</summary>
        private static readonly Dictionary<string, List<string>> KeywordResponses =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
            {
                "Use strong passwords with 12+ characters — mix uppercase, lowercase, numbers, and symbols. Never reuse passwords across sites! Consider a password manager like Bitwarden or 1Password. 🔐",
                "A strong password should be at least 12 characters and avoid dictionary words. Try a passphrase: three random words joined together can be surprisingly secure! 🛡️",
                "Password managers generate and store unique passwords for every account, so you only need to remember one master password. It's one of the best security habits you can build! 🔑"
            },
                ["phishing"] = new List<string>
            {
                "Watch for suspicious emails, texts, or calls. Never click unknown links or download attachments from untrusted sources. Always verify the sender's address carefully! 🎣",
                "Phishing emails often create urgency — 'Act now or your account will be suspended!' Real companies rarely pressure you like this. When in doubt, go directly to the official website. ⚠️",
                "Check the sender's actual email address, not just the display name. Scammers can show 'Amazon Support' while the real address is something suspicious@random.com. 🔍"
            },
                ["scam"] = new List<string>
            {
                "Never send money, gift cards, or cryptocurrency to someone you haven't met in person. Legitimate organisations will never ask for payment this way. 🚨",
                "Common scam signs: unexpected contact, too-good-to-be-true offers, urgency, and requests for personal information or payment. Always trust your gut! 🛑",
                "Report scams to your national cybercrime authority. Your report can protect others from falling victim to the same scheme! 🛡️"
            },
                ["privacy"] = new List<string>
            {
                "Review app permissions regularly — does your flashlight app really need your contacts? Limit permissions to only what's necessary for the app to function. 🔒",
                "Use a VPN on public Wi-Fi to encrypt your traffic. Avoid accessing sensitive accounts (banking, email) on public networks when possible. 🌐",
                "Check your social media privacy settings regularly. Oversharing your address, workplace, or daily routine can be exploited by bad actors. 📱"
            },
                ["malware"] = new List<string>
            {
                "Keep your antivirus software updated and run regular scans. Malware can lurk undetected, so scheduled scans are essential for catching threats early. 🦠",
                "Download software only from official, verified sources. Cracked or pirated software is a major malware delivery vector — the 'free' version often costs you far more! 💀",
                "Signs of malware infection: slow performance, unexpected pop-ups, programs launching on their own, or unusual network activity. Run a full scan immediately! ⚡"
            },
                ["2fa"] = new List<string>
            {
                "Two-Factor Authentication (2FA) blocks 99.9% of automated account takeover attacks! Enable it on every account — especially email, banking, and social media. 🛡️",
                "Authenticator apps like Google Authenticator or Authy are more secure than SMS codes, since phone numbers can be hijacked via SIM-swapping attacks. 📲",
                "Hardware security keys (like YubiKey) are the gold standard for 2FA — they're phishing-resistant because they verify the actual website you're visiting. 🔑"
            },
                ["mfa"] = new List<string>
            {
                "Multi-Factor Authentication (MFA) uses two or more verification methods: something you know (password), something you have (phone), or something you are (fingerprint). 🛡️",
                "Even if your password is stolen, MFA stops attackers in their tracks. It is one of the highest-impact security measures any individual can implement today. ✅"
            },
                ["vpn"] = new List<string>
            {
                "A VPN encrypts your internet traffic and hides your IP address. Use one on public Wi-Fi — coffee shops, airports, and hotels are prime spots for eavesdropping. 🔒",
                "Choose a VPN with a strict no-logs policy that has been independently audited. Free VPNs often monetise your data — you become the product! 💡",
                "VPNs are excellent for privacy, but they're not a magic shield. Combine with strong passwords and 2FA for truly layered protection. 🌐"
            },
                ["backup"] = new List<string>
            {
                "Follow the 3-2-1 rule: keep 3 copies of your data, on 2 different media types, with 1 stored offsite (cloud storage works great for this!). 💾",
                "Test your backups regularly! A backup you've never restored is a backup you can't trust. Schedule monthly restore tests to verify your data is recoverable. ✅",
                "Ransomware can encrypt everything connected to your machine — including mapped network drives. Keep at least one backup completely offline or air-gapped. 🛡️"
            },
                ["ransomware"] = new List<string>
            {
                "Ransomware encrypts your files and demands payment for the decryption key. The best defence is regular offline backups so you can restore without paying criminals. 💾",
                "Never pay ransomware demands if you can avoid it — payment doesn't guarantee you'll get your files back, and it funds further criminal attacks. Contact cybersecurity professionals first! 🚫",
                "Prevention is key: keep all software updated, avoid suspicious downloads, use email filtering, and maintain at least one offline backup at all times. 🔐"
            },
                ["update"] = new List<string>
            {
                "Software updates patch known security vulnerabilities. Delaying them leaves a door wide open for attackers who specifically target unpatched systems. ⚡",
                "Enable automatic updates for your operating system, browsers, and key applications. Most successful attacks exploit vulnerabilities that already have patches available! 🔄",
                "Don't forget firmware updates for your router and IoT devices. Smart devices are frequently neglected and can serve as easy entry points into your entire network. 📡"
            },
                ["wifi"] = new List<string>
            {
                "Public Wi-Fi networks are a hacker's playground. Always use a VPN when connecting, and avoid accessing sensitive accounts on public networks. 📶",
                "Secure your home Wi-Fi with WPA3 encryption (or WPA2 if WPA3 is unavailable). Change default router passwords and consider disabling WPS for better security. 🏠",
                "Rogue hotspots mimic legitimate networks (e.g., 'Airport_Free_WiFi'). Always verify the exact network name with staff before connecting in public spaces. 📡"
            },
                ["social"] = new List<string>
            {
                "Social engineering manipulates people rather than systems. Attackers may impersonate colleagues, IT support, or executives to trick you into revealing sensitive information. 🎭",
                "Be suspicious of any unsolicited contact asking for credentials, money transfers, or sensitive data — even if the person seems legitimate. Always verify via a separate channel! 🔍",
                "Oversharing on LinkedIn or other social media helps attackers craft convincing spear-phishing emails targeted specifically at you. Audit what's publicly visible! 🌐"
            }
            };

        /// <summary>Sentiment keywords for detection (Sentiment Detection requirement).</summary>
        private static readonly Dictionary<string, List<string>> SentimentMap =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["worried"] = new List<string> { "worried", "scared", "anxious", "afraid", "nervous", "concerned", "frightened", "unsafe", "fear", "terrified" },
                ["curious"] = new List<string> { "curious", "interested", "wondering", "fascinated", "intrigued", "want to know", "how does", "what is", "explain" },
                ["frustrated"] = new List<string> { "frustrated", "annoyed", "confused", "don't understand", "complicated", "difficult", "ugh", "hate", "can't figure", "overwhelmed" },
                ["happy"] = new List<string> { "great", "thanks", "thank you", "awesome", "love", "amazing", "helpful", "brilliant", "perfect", "excellent" }
            };

        /// <summary>Follow-up phrases for Conversation Flow requirement.</summary>
        private static readonly List<string> FollowUpPhrases = new List<string>
        {
            "tell me more","more","another tip","give me another","explain more",
            "more details","elaborate","go on","continue","what else","anything else","tell me again"
        };

        private static readonly List<string> Greetings = new List<string> { "hello", "hi", "hey", "greetings", "good morning", "good afternoon", "good evening", "howdy" };
        private static readonly List<string> ExitWords = new List<string> { "exit", "quit", "bye", "goodbye", "farewell", "see you" };

        // ═══════════════════════════════════════════════════════════
        //  SESSION STATE  (Memory & Recall requirement)
        // ═══════════════════════════════════════════════════════════

        private string _userName = "";
        private string _lastTopic = "";       // Conversation flow context
        private int _emptyCount = 0;
        private readonly List<string> _rememberedInterests = new List<string>(); // Memory store
        private readonly Random _rng = new Random();

        // ═══════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ═══════════════════════════════════════════════════════════

        public MainWindow()
        {
            InitializeComponent();
            PlayVoiceGreeting();
            NameTextBox.Focus();
        }

        // ═══════════════════════════════════════════════════════════
        //  VOICE GREETING  (Part 1 carry-over)
        // ═══════════════════════════════════════════════════════════

        private void PlayVoiceGreeting()
        {
            try
            {
                string resourceName = "CyberSecurity_Bot.greeting.wav";
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        var player = new SoundPlayer(stream);
                        player.Play();
                    }
                }
            }
            catch { /* Silently skip if WAV not present */ }
        }

        // ═══════════════════════════════════════════════════════════
        //  NAME ENTRY
        // ═══════════════════════════════════════════════════════════

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) StartSession();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartSession();
        }

        private void StartSession()
        {
            string raw = NameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(raw)) return;

            // Capitalise first letter
            _userName = char.ToUpper(raw[0]) + raw.Substring(1).ToLower();

            // Hide name panel, enable input
            NamePanel.Visibility = Visibility.Collapsed;
            UserInput.IsEnabled = true;
            SendBtn.IsEnabled = true;

            // Update header badges
            UserBadgeText.Text = $"👤 {_userName}";
            UserBadge.Visibility = Visibility.Visible;

            // Post welcome message
            AddBotMessage(
                $"🌟 Welcome, {_userName}! I'm BIMO, your Cybersecurity Awareness Assistant.\n\n" +
                $"I can help you with: passwords, phishing, scams, privacy, malware, 2FA/MFA, VPNs, backups, ransomware, updates, Wi-Fi safety, and social engineering.\n\n" +
                $"💡 Click a topic button below, type naturally, or type 'help' for all commands! 🛡️",
                MessageType.Welcome);

            UserInput.Focus();
        }

        // ═══════════════════════════════════════════════════════════
        //  INPUT HANDLING
        // ═══════════════════════════════════════════════════════════

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ProcessAndSend();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessAndSend();
        }

        /// <summary>Quick-topic chip buttons feed directly into the pipeline.</summary>
        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string chip = btn.Content?.ToString() ?? "";
                AddUserMessage(chip);
                ProcessInput(chip);
            }
        }

        private void ProcessAndSend()
        {
            string raw = UserInput.Text.Trim();
            UserInput.Clear();

            // Empty input handling (Error Handling requirement)
            if (string.IsNullOrWhiteSpace(raw))
            {
                _emptyCount++;
                if (_emptyCount >= 3)
                {
                    AddBotMessage($"I notice you're not typing anything, {_userName}. Try clicking a topic chip or ask about passwords, phishing, or type 'help'! 💡", MessageType.Tip);
                    _emptyCount = 0;
                }
                else
                {
                    AddBotMessage($"Please type something, {_userName}! I'm here to help. 😊");
                }
                return;
            }

            _emptyCount = 0;
            AddUserMessage(raw);
            ProcessInput(raw);
        }

        // ═══════════════════════════════════════════════════════════
        //  CORE PROCESSING PIPELINE
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Central routing method. Checks inputs in priority order:
        /// exit → utility commands → conversational → memory → follow-up → keyword → sentiment → default.
        /// </summary>
        private void ProcessInput(string raw)
        {
            // Length guard (Error Handling)
            if (raw.Length > 300)
            {
                AddBotMessage($"That's a very long message, {_userName}! Could you break it into a shorter question? 😅");
                return;
            }

            string lower = raw.ToLower().Trim();
            // Sanitise input
            lower = Regex.Replace(lower, @"[<>]", "");
            lower = Regex.Replace(lower, @"\s+", " ");

            // ── Exit ────────────────────────────────────────────────
            if (ExitWords.Any(w => lower == w || lower.StartsWith(w + " ")))
            {
                AddBotMessage($"Stay safe online, {_userName}! Cybersecurity is everyone's responsibility. Goodbye! 👋", MessageType.Goodbye);
                UserInput.IsEnabled = false;
                SendBtn.IsEnabled = false;
                return;
            }

            // ── Help ────────────────────────────────────────────────
            if (lower == "help" || lower == "commands")
            {
                AddBotMessage(
                    "📋 AVAILABLE COMMANDS\n\n" +
                    "🔐 Security Topics:\npassword · phishing · scam · privacy · malware · 2fa · mfa · vpn · backup · ransomware · update · wifi · social\n\n" +
                    "💬 Conversational:\nhello · how are you · what's your purpose · thank you · topics · who made you\n\n" +
                    "🧠 Memory:\n\"I'm interested in [topic]\"  ·  \"what do you remember about me\"\n\n" +
                    "🔄 Follow-up:\n\"tell me more\" · \"another tip\" · \"explain more\" · \"what else\"\n\n" +
                    "🛠️ Utility:\nhelp · topics · exit / bye",
                    MessageType.Help);
                return;
            }

            // ── Topics list ─────────────────────────────────────────
            if (lower == "topics" || lower == "what can i ask" || lower == "what do you know")
            {
                AddBotMessage(
                    "📚 TOPICS I COVER:\n\n" +
                    "🔐 Passwords & passphrases\n🎣 Phishing & email scams\n💸 Online scams\n" +
                    "🔒 Privacy settings\n🦠 Malware & viruses\n🛡️ Two-Factor Auth (2FA/MFA)\n" +
                    "🌐 VPNs & public Wi-Fi\n💾 Backups (3-2-1 rule)\n💀 Ransomware\n" +
                    "⚡ Software updates\n📶 Wi-Fi security\n🎭 Social engineering\n\n" +
                    "Type any topic or click a chip below!",
                    MessageType.Topics);
                return;
            }

            // ── How are you ─────────────────────────────────────────
            if (lower.Contains("how are you") || lower == "how r u")
            {
                var replies = new List<string>
                {
                    $"I'm doing great, {_userName}! All security protocols running smoothly. How can I help? 🛡️",
                    $"Excellent! Just analysing threat landscapes and keeping users like you informed. What's on your mind? 🔒",
                    $"Fantastic, thanks for asking! Ready to help you stay safe online. What would you like to know? 💡"
                };
                AddBotMessage(GetRandom(replies));
                return;
            }

            // ── Purpose / identity ──────────────────────────────────
            if (lower.Contains("your purpose") || lower.Contains("why do you exist") || lower.Contains("what are you"))
            {
                AddBotMessage($"My purpose, {_userName}, is to educate and empower users like you to stay safe online! I cover everything from password safety to ransomware defence. Think of me as your personal digital bodyguard. 🛡️");
                return;
            }

            // ── Who made you ────────────────────────────────────────
            if (lower.Contains("who made you") || lower.Contains("who created you") || lower.Contains("your creator"))
            {
                AddBotMessage($"I was created by cybersecurity experts to raise awareness about online safety, {_userName}! My mission is to make the digital world safer for everyone. 🌐");
                return;
            }

            // ── Greetings ───────────────────────────────────────────
            if (Greetings.Any(g => lower == g || lower.StartsWith(g + " ") || lower.StartsWith(g + "!")))
            {
                var greets = new List<string>
                {
                    $"Hello again, {_userName}! Ready to learn about cybersecurity? 🛡️",
                    $"Hey {_userName}! How can I help protect your digital life today? 🔒",
                    $"Hi {_userName}! Staying safe online starts with awareness — let's learn together! 💡"
                };
                AddBotMessage(GetRandom(greets));
                return;
            }

            // ── Thank you ───────────────────────────────────────────
            if (lower.Contains("thank") || lower == "thx" || lower == "cheers")
            {
                AddBotMessage($"You're very welcome, {_userName}! Stay vigilant out there. 😊 Is there anything else you'd like to know about cybersecurity?");
                return;
            }

            // ── Memory: store an interest ───────────────────────────
            var interestMatch = Regex.Match(lower, @"i(?:'m| am) (?:interested in|curious about|worried about) (.+)");
            if (interestMatch.Success)
            {
                string topic = interestMatch.Groups[1].Value.TrimEnd('.', '!', '?').Trim();
                if (!_rememberedInterests.Contains(topic))
                    _rememberedInterests.Add(topic);
                UpdateMemoryBadge();
                AddBotMessage(
                    $"Great! I'll remember that you're interested in {topic}, {_userName}. It's a crucial part of staying safe online.\n\n" +
                    $"Here's a key tip: regularly review your security settings and stay updated on the latest threats in this area! 🔒",
                    MessageType.Memory);
                return;
            }

            // ── Memory: recall ──────────────────────────────────────
            if (lower.Contains("what do you remember") || lower.Contains("do you remember me") || lower.Contains("my interests"))
            {
                if (_rememberedInterests.Count > 0)
                {
                    string list = string.Join("\n", _rememberedInterests.Select(i => $"• {i}"));
                    AddBotMessage($"Of course, {_userName}! 🧠\n\nI remember you've shown interest in:\n{list}\n\nWould you like more tips on any of these?", MessageType.Memory);
                }
                else
                {
                    AddBotMessage($"I know your name is {_userName}! You haven't shared specific interests yet. Try saying \"I'm interested in privacy\" or just ask about any cybersecurity topic! 🤖");
                }
                return;
            }

            // ── Follow-up conversation flow ─────────────────────────
            if (FollowUpPhrases.Any(p => lower.Contains(p)) && !string.IsNullOrEmpty(_lastTopic))
            {
                if (KeywordResponses.TryGetValue(_lastTopic, out var followResponses))
                {
                    string sentiment = DetectSentiment(lower);
                    string prefix = BuildSentimentPrefix(sentiment);
                    string tip = GetRandom(followResponses);
                    string memNote = _rememberedInterests.Count > 0
                        ? $"\n\n💡 Since you've been exploring {_lastTopic}, this is especially relevant for you!"
                        : "";
                    AddBotMessage(prefix + tip + memNote);
                    return;
                }
            }

            // ── Keyword detection (partial / substring match) ───────
            string matchedKeyword = DetectKeyword(lower);
            if (matchedKeyword != null)
            {
                string sentiment = DetectSentiment(lower);
                string prefix = BuildSentimentPrefix(sentiment);
                string response = GetRandom(KeywordResponses[matchedKeyword]);
                string memNote = _rememberedInterests.Contains(matchedKeyword)
                    ? $"\n\n🧠 I remember you're particularly interested in {matchedKeyword} — here's something especially useful!"
                    : "";

                _lastTopic = matchedKeyword;
                if (!_rememberedInterests.Contains(matchedKeyword))
                {
                    _rememberedInterests.Add(matchedKeyword);
                    UpdateMemoryBadge();
                }

                AddBotMessage(prefix + response + memNote);

                // Secondary follow-up prompt (Conversation Flow)
                AddBotMessage($"💡 Tip: Type \"tell me more\" for another {matchedKeyword} tip, or ask about any other cybersecurity topic!", MessageType.Tip);
                return;
            }

            // ── Sentiment-only fallback ─────────────────────────────
            string onlySentiment = DetectSentiment(lower);
            if (onlySentiment == "worried")
            {
                AddBotMessage(
                    $"It's completely understandable to feel worried, {_userName}. Scammers and hackers can be very convincing.\n\n" +
                    $"Knowledge is your best defence! Ask me about phishing, scams, or passwords to start building your protection. 🛡️");
                return;
            }
            if (onlySentiment == "frustrated")
            {
                AddBotMessage(
                    $"I hear you, {_userName} — cybersecurity can feel overwhelming at first. Let's take it one step at a time.\n\n" +
                    $"What specific topic would you like me to simplify? Try clicking 'password', 'phishing', or '2fa' in the chips above! 😊");
                return;
            }

            // ── Question-shaped input ───────────────────────────────
            var qStarters = new[] { "what", "how", "why", "when", "where", "who", "is", "can", "does", "should" };
            if (qStarters.Any(q => lower.StartsWith(q)) || lower.EndsWith("?"))
            {
                var replies2 = new List<string>
                {
                    $"That's an interesting question, {_userName}, but I specialise in cybersecurity topics. Try asking about passwords, phishing, scams, privacy, malware, 2FA, VPNs, or backups! Type 'help' to see all options.",
                    $"I'm focused on cybersecurity awareness, {_userName}. Could you rephrase around a security topic? Type 'topics' to see everything I cover!"
                };
                AddBotMessage(GetRandom(replies2));
                return;
            }

            // ── Default fallback (Error Handling requirement) ────────
            var defaults = new List<string>
            {
                $"I didn't quite catch that, {_userName}. I specialise in cybersecurity! Try asking about passwords, phishing, scams, privacy, malware, 2FA, VPNs, or backups. Type 'help' for all options.",
                $"Hmm, that's outside my expertise, {_userName}. Ask me about a cybersecurity topic — type 'topics' to see everything I cover!",
                $"I don't recognise that query, {_userName}. Feel free to ask about any cybersecurity topic, or type 'help' to see what I can do. 🛡️"
            };
            AddBotMessage(GetRandom(defaults));
        }

        // ═══════════════════════════════════════════════════════════
        //  HELPER METHODS  (Code Optimisation)
        // ═══════════════════════════════════════════════════════════

        /// <summary>Detect first matching keyword using substring (partial) match.</summary>
        private static string DetectKeyword(string lower)
        {
            foreach (var kw in KeywordResponses.Keys)
                if (lower.Contains(kw)) return kw;
            return null;
        }

        /// <summary>Detect sentiment from a list of sentiment indicator words.</summary>
        private static string DetectSentiment(string lower)
        {
            foreach (var kvp in SentimentMap)
                if (kvp.Value.Any(w => lower.Contains(w)))
                    return kvp.Key;
            return "neutral";
        }

        /// <summary>Build empathetic prefix based on detected sentiment.</summary>
        private string BuildSentimentPrefix(string sentiment)
        {
            switch (sentiment)
            {
                case "worried": return $"It's completely understandable to feel that way, {_userName}. You're taking a great first step by learning about this! ";
                case "curious": return $"Love the curiosity, {_userName}! Here's what you should know: ";
                case "frustrated": return $"I hear you, {_userName} — this can feel overwhelming. Let me break it down simply: ";
                case "happy": return $"Glad to help, {_userName}! ";
                default: return "";
            }
        }

        /// <summary>Return a random item from a list.</summary>
        private T GetRandom<T>(List<T> list) => list[_rng.Next(list.Count)];

        /// <summary>Keep memory badge in sync with stored interests.</summary>
        private void UpdateMemoryBadge()
        {
            if (_rememberedInterests.Count > 0)
            {
                MemoryBadgeText.Text = $"🧠 {_rememberedInterests.Count} remembered";
                MemoryBadge.Visibility = Visibility.Visible;
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  UI HELPERS — Add messages to chat panel
        // ═══════════════════════════════════════════════════════════

        private enum MessageType { Normal, Welcome, Tip, Memory, Help, Topics, Goodbye }

        private void AddUserMessage(string text)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(40, 4, 4, 4) };

            var bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x0D, 0x20, 0x40)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0x60, 0xCC)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(11, 3, 11, 11),
                Padding = new Thickness(12, 8, 12, 8),
                MaxWidth = 580
            };

            bubble.Child = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0x98, 0xC8, 0xFF)),
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap
            };

            panel.Children.Add(bubble);
            panel.Children.Add(new TextBlock { Text = "👤", FontSize = 18, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(6, 0, 0, 0) });

            AnimateAndAdd(panel);
        }

        private void AddBotMessage(string text, MessageType type = MessageType.Normal)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(4, 4, 40, 4) };

            panel.Children.Add(new TextBlock { Text = "🤖", FontSize = 18, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 6, 0) });

            // Border colour by message type
            Color borderCol = type switch
            {
                MessageType.Welcome => Color.FromRgb(0x00, 0xCC, 0x66),
                MessageType.Tip => Color.FromRgb(0xCC, 0xAA, 0x00),
                MessageType.Memory => Color.FromRgb(0x00, 0x88, 0xCC),
                MessageType.Help => Color.FromRgb(0x88, 0x44, 0xCC),
                MessageType.Topics => Color.FromRgb(0x00, 0xCC, 0xAA),
                MessageType.Goodbye => Color.FromRgb(0xCC, 0x44, 0x44),
                _ => Color.FromRgb(0x1A, 0x4A, 0x2A)
            };

            Color bgCol = type switch
            {
                MessageType.Welcome => Color.FromRgb(0x04, 0x10, 0x08),
                MessageType.Tip => Color.FromRgb(0x10, 0x0E, 0x00),
                MessageType.Memory => Color.FromRgb(0x00, 0x08, 0x14),
                MessageType.Help => Color.FromRgb(0x08, 0x04, 0x14),
                MessageType.Topics => Color.FromRgb(0x00, 0x10, 0x0E),
                MessageType.Goodbye => Color.FromRgb(0x14, 0x04, 0x04),
                _ => Color.FromRgb(0x06, 0x10, 0x08)
            };

            var inner = new StackPanel();

            // Label
            inner.Children.Add(new TextBlock
            {
                Text = "🤖 BIMO",
                Foreground = new SolidColorBrush(Color.FromArgb(0x88, 0x00, 0xFF, 0x88)),
                FontFamily = new FontFamily("Consolas"),
                FontSize = 10,
                Margin = new Thickness(0, 0, 0, 3)
            });

            // Message text
            inner.Children.Add(new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0xC6, 0xFF, 0xDA)),
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap
            });

            var bubble = new Border
            {
                Background = new SolidColorBrush(bgCol),
                BorderBrush = new SolidColorBrush(borderCol),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3, 11, 11, 11),
                Padding = new Thickness(12, 8, 12, 8),
                MaxWidth = 580,
                Child = inner
            };

            panel.Children.Add(bubble);
            AnimateAndAdd(panel);
        }

        /// <summary>Fade-in animation on new message and auto-scroll.</summary>
        private void AnimateAndAdd(UIElement element)
        {
            element.Opacity = 0;
            MessagesPanel.Children.Add(element);

            var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(220));
            element.BeginAnimation(OpacityProperty, anim);

            // Scroll to bottom
            ChatScroller.ScrollToBottom();
        }
    }
}
