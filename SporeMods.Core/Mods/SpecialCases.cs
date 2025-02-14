﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace SporeMods.Core.Mods
{
    public static class SpecialCases
    {
        static readonly IReadOnlyList<string> HAZARDOUS_LOOSE_PACKAGE_NAMES
            = new List<string>()
            {
                //450 mods pack
                "allsandbox",
                "Tighonsecurity",
                "50592-earlypb",
                "boosted_idrive_2x",
                "NoAngerAlly",
                "33227-blazesRusmod",
                "13023-faster_ufo",
                "67181-infinitepartsize",
                "20362-gravity_hammer",
                "DavoBabyKidnap",
                "epicplay2.1",
                "14804-geneticmodificationGA",
                "genesis-atmospheric-tool",
                "Spacelord_2",
                "Rogueplay1.0",
                "30298-BetterTurrets",
                "11289-DavoElectricCellParts",
                "more_spucks_normal",
                "road_runner_v3",
                "wastedisposal",
                "killing_time_lite",
                "moreepics",
                "KayixWandererBKG",
                "semgrox",
                "89163-cleverfood5",
                "geneticmodification",
                "Quit first mission. Reward 5m",
                "37111-A_No_Joker_Badge_Mod",
                "Grox_made_easy",
                "Bio_Cakewalk",
                "Uber_Missiles",
                "76568-DavoWildlifeMod",
                "CreatureOverhaul",
                "slavery",
                "3695-14UltraAntimatter",
                "Strong Allies",
                "Mod_Difficulty_Tuning",
                "quicker colonies",
                "Mega Death Bomb",
                "staffofice",
                "packratcargo500",
                "improvedcomplexity",
                "Bigger_Systems1-15",
                "staffofspode",
                "blazesRusmod",
                "more_spucks_easy",
                "CostTravel",
                "levu",
                "Mod_Kittani_Players_Choice",
                "mod_grox",
                "26774-4DPlanetsbyDavo",
                "Spodebomb",
                "SuperCannons",
                "76694-differentvillagenames",
                "BiggerPlanets2_1",
                "civepics",
                "23172-floratest",
                "50687-100planetsmod",
                "EPICWARZ",
                "XionicSingularityMissles",
                "enhancedPulse_Tools",
                "!Tools of War!",
                "Spice_Overhaul_v1",
                "sporegod",
                "Mod_Space_Travel",
                "epicallies",
                "28504-alteredturretweapons",
                "84767-easybadges",
                "97245-EmptyGalaxy",
                "HardCore_Action_Spore",
                "machinegun",
                "30797-DavoCreatureOverhaulV1",
                "colonyfever",
                "Farseer",
                "odistant_nuclear_desolation_asteroid_nuclear",
                "2508-epicplay2.1",
                "MoneyRay",
                "91593-noJokerBadge",
                "91190-WhosInCharge",
                "86787-Sell_high-Buy_free",
                "warpdrive",
                "NoAnger",
                "TheMegaDud",
                "70115-richSpore",
                "33015-Lower Grox UFOs",
                "60734-14CreatureEditStatFreedom",
                "1Warlord_Rally",
                "I_want_spice",
                "86965-UbErMoD",
                "New_Archetypes",
                "odistant_sharp_points",
                "57822-DragonWingasymmetrymod",
                "Pulse_Tools",
                "42_genesisrenamenoexitnounlimited",
                "Better colony",
                "darkmattervortex",
                "41797-500cargo",
                "civstage",
                "21249-cleverfood",
                "79370-newspices",
                "Mod_Super_Kryjack_Wep_pack",
                "Colonizer_Suite090",
                "aIntense_Combat",
                "Galacticcodenobreak",
                "mod_flight",
                "Dark Orbit Spore",
                "SocializeArmy",
                "AnEpic1.2",
                "BetterSpore",
                "65034-placetribaltoolsanywhere",
                "MilitaryArmy",
                "Starwarstools1",
                "35652-4DPlanetsbyDavo",
                "FasterUFO",
                "50959-UbErMoD",
                "BigUFO",
                "unhappyray",
                "lessempires",
                "16134-groxinlava",
                "41108-DavosRoboticAquatic",
                "32061-500bagdes",
                "veryhard_difficulty",
                "71110-ColonyMod",
                "mod_cargo",
                "69895-richSpore",
                "Easy badges",
                "Stardragon231WarriorBKG",
                "Sea_Monster_Begone",
                "hell",
                "Mountains",
                "BecomeAOmnipotent",
                "Vehicle Hacking",
                "energybomb",
                "GroxCityReplacer",
                "AnyToolOnAnyPlanet",
                "SpaceSandbox",
                "EasyGoingEmpires",
                "R2-D2",
                "Quit first mission. Reward 1m",
                "Steve's_Lottery_Ticket",
                "road_runner_v2",
                "17365-building",
                "70758-aaa1",
                "43977-Grox archetype",
                "14037-bettersporeexpansionpak",
                "galaxyshredder",
                "71043-Bigger_Systems1-15",
                "UBERturret",
                "health_and_wealth_normal",
                "mayflower",
                "battleship",
                "Easy_Tribalstage",
                "PowerfulPulse_Tools",
                "BiggerPlanets",
                "Staf_of_static",
                "Quit first mission. Reward 0",
                "Spacelord",
                "Intense_Combat",
                "14681-bigplanets2x", //has 2 .package extensions in the 450 mods pack
                "5278-AToAP Infinity 2.1",
                "cruiserflak",
                "Lower Grox UFOs",
                "39606-14UltraAntimatter",
                "atriberefresh",
                "UUs_weapon_addon",
                "23836-fixinterstellar",
                "51935-cleverfruit",
                "79217-BP2_Data",
                "UUs_weapon_tools1.2",
                "EcoMutationLaC",
                "PlusHealthMoney",
                "81897-ufo2Xhealth",
                "60685-UbErMoD",
                "Diffuculttime",
                "AllArchetypes",
                "PlatinumSpore_1.2",
                "150Wildlife_v1",
                "firedy",
                "fanaticalfury",
                "odistant_nuclear_desolation_asteroid",
                "JammyLongScientistBKG",
                "longercreaturestage",
                "37002-captaincargo50",
                "Galactic Fredom",
                "radiationrally",
                "MoreTools",
                "Addon_Glasses_1.04_compatible",
                "cloaknshield",
                "staffofgrox",
                "floratest",
                "gravity_hammer",
                "rainforestvacuum",
                "more_health",
                "badge_mania_ultra",
                "nohazards",
                "30592-WTF badge",
                "50654-cleverfood",
                "aGeneticmod+freedom",
                "Full_colony_space_v1",
                "44023-WhosInCharge",
                "EcoMutationLaB",
                "decorations",
                "4888-damagingheatray",
                "10716-SpiceStorage",
                "21494-No_cooldown",
                "96014-2005civstage",
                "Atmospheretools (1)",
                "addon_glasses",
                "OmegaWeapons",
                "94118-floratest",
                "52542-planet_buster_camera_tweak",
                "ShineLikeAStarTraderBKG",
                "Eatingwithmouth",
                "Galaxy_Mod",
                "22793-geneticmodification2editors",
                "92567-megaturret",
                "1StarFleet",
                "more_money_for_me",
                "PotenciaMaxima",
                "groxloveyouMOD",
                "Grox_archetype",
                "Black Cloud",
                "moremission",
                "14122-groxinstantkiller",
                "59829-14CreatureEditStatFreedom",
                "SSD Flak Cannon",
                "stardust",
                "improvedinterstellerdrive",
                "odistant_i-wanna-level-faster",
                "Better Spore1.2",
                "55962-DavoElectricCellParts",
                "HappyRay-Cap200",
                "46722-unlimitedSoL",
                "Super_jump",
                "civgod",
                "Allepics1.0",
                "mod_noopenmovie",
                "editoreffectsingame",
                "alltoolsbetterspore",
                "81217-unlimitedcolonypacks",
                "Terraweapons",
                "Nuculear_Chaingun",
                "95416-warpdrive",
                "Better Spore",
                "9617-14CreatureEditStatFreedom",
                "allParts",
                "Atmospheretools",
                "colony_wildlife_v1",
                "35421-LongerCreatureGame",
                "sculptistsdream",
                "22818-hazardmodification", //has 2 .package extensions in the 450 mods pack
                "Archetype",
                "62574-HydroRepulse",
                "43598-noJokerBadge",
                "91909-permananttemperaturechanges",
                "health_and_wealth_hard",
                "''1337'' Weapons",
                "Redesigned_Captain_PartsV1.2",
                "55236-grox",
                "skywriter",
                "atriberefresh (1)",
                "play_as_an_epic",
                "CC Scale Limits",
                "75758-14StartingArtists",
                "MoreColonySpicex2",
                "48265-DavosLiquidCell",
                "42",
                "21053-giant_Parts",
                "hurricane",
                "killing_time",
                "MorePlanetSpice99",
                "powerweopons",
                "richmansgalaxymod",
                "start_with_everything",
                "16884-DavoFlightModMoon",
                "Sea_Voyages",
                "PlatinumSpore",
                "87001-grox10planets",
                "1843-no_grox",
                "BiggerPlanets2",
                "82496-Galacticcodenobreak",
                "DavosRoboticAquatic",
                "38769-BRR mod",
                "ufo2Xhealth",
                "59316-Infinite_SoL",
                "40morespace",
                "50Wildlife_v1",
                "61901-50%FewerEmpires",
                "88191-PositiveGroxAlliance",
                "ThobewillTraderBKG",
                "4718-toolMod",
                "Xionic_Omega_Device",
                "no_grox",
                "82273-DavoElectricCellParts",
                "75214-14FirstTerraGreen",
                "60415-DavoElectricCellParts",
                "The_God_That_Will_Come's_Darkness",
                "More_Storybook",
                "87455-groxer",
                "55538-14CreatureEditStatFreedom",
                "57762-DavoFlightMod",
                "groxloveyouMOD1.2",
                "23903-nohazards",
                "500bagdes",
                "TheTrueDerajKnightBKG",
                "PdmjpdmjShamanBKG",
                "Space_cocaine",
                "44391-allsuper_weapons",
                "MorePlanetSpicex2",
                "megapack",
                "party",
                "Staticshock",
                "weaponsofthenewera",
                "lifemod",
                "6466-Space Cocaine",
                "Fantastic Voyage",
                "joked_joker",
                "70043-DavoColorBots",
                "59496-shop_till_you_drop",
                "spaceisfunhack",
                "22882-DavoBabyKidnap",
                "58657-DavoStaticCharge",
                "level 6",
                "LessTime",
                "healthbomb",
                "ColonyCapture",
                "ChimerahoundZealotBKG",
                "atechtree",
                "1337er Uber Turrets",
                "tac",
                "Sphat",
                "27555-no_ammo_mod",
                "Hardtribe",
                "46719-Grox archetype",
                "difficultytweak",
                "GigerSporeVirak1",
                "recycler",
                "Mod_Kittani_Players_Choice_Plus",
                "Black GUI 1.2",
                "orbsofdoom",
                "No_Energy_Captain_Parts",
                "FV Weapons Upgrade",
                "nogrox",
                "Spectrum_Beam",
                "Spooce_Lizard's_Hypnobeam",
                "mrfusion",
                "Galacticgore",
                "biorestore",
                "health_and_wealth_easy",
                "graveyard",
                "Strong_and_healthy_allies",
                "The_Groxs'_Flames",
                "HappyRay-Cap1200",
                "All_Unit_Types",
                "EaglejediTraderBKG",
                "lotsofhealthspace",
                "ecodisaster",
                "Quick Trade Routes",
                "96849-hazardvariety",
                "colonyfever_balanced",
                "buy all terraform specials",
                "EcoMutationSmB",
                "Deflect",
                "36209-PAGA",
                "14462-halfgrox",
                "aaScatteredv2.0",
                "54308-citywalls",
                "mod_noopenmovie_invincible",
                "Angorak_Missile_turret",
                "epicplay1.0",
                "33386-cellPartsXtended",
                "59293-infinitepartsize",
                "SpaceRealism",
                "moneymoneymoney",
                "AToAP Infinity 2.1",
                "82891-Space Cocaine",
                "asymmetry",
                "83161-no_ammo_mod",
                "antigrox",
                "ColonyMod",
                "FewerBiggerEmpires",
                "21288-DavoFlightModMoon",
                "odistant_destructive_asteriod",
                "StarFleet",
                "cheatehmowad",
                "Ultimate Weapon Pack",
                "92338-CreatureTestOX",
                "13863-DavoGroxEditor",
                "BetterSpore_AllParts",
                "Large_Scale_Tribal_Stage_V1",
                "planet buster laser",
                "plaguefist",
                "MoreColonySpice98",
                "ChimerahoundZealotBKG (1)",
                "78568-biggerspaceships",
                "AlienVirakSmiles",
                "HUD",
                "58456-DavoPlanetFlight",
                "24273-geology",
                "Kaploy9KnightBKG",
                "geneticmodificationGA",
                "odistant_civilization_ruler",
                "42_GenesisDevice",
                "groxparts",
                "jk",
                "Spode's_light",
                "HardTribal_made_easy",
                "rechargeplus2traderoutes",
                "space_icons",
                "Turret_Defender",
            }
        .AsReadOnly();


        static readonly IReadOnlyList<string> FILTER_CHARS
            = new List<string>{
                string.Empty,
                "_",
                " ",
                "-",
                " "
            }
        .AsReadOnly();

        static readonly IReadOnlyList<string> HAZARDOUS_LOOSE_PACKAGE_FILTERS
            = CreateFilters(new List<string>()
            {
                //things people keep seeing due to search engine optimization
                "*Better?Spore*",
                "*Buzz?Sphere*",
                "*Buzz?Orbit*",
                "*Orbit?Spore*",
                "*Dark?Orbit*",
                "*Realor?Spore*",
                "*Dusk?Spore*",
                "*Forgotten?Spore*",
                "*Titan?Spore*",
                "*Platinum?Spore*",
            }
        );

        static IReadOnlyList<string> CreateFilters(List<string> filtersToCreate)
        {
            List<string> filters = new List<string>();
            foreach (string filterToCreate in filtersToCreate)
            {
                foreach (string filterReplace in FILTER_CHARS)
                    filters.Add(filterToCreate.Replace("?", filterReplace));
            }
            return filters.AsReadOnly();
        }


        public static bool HazardMatchLoosePackage(string fileName, out bool matchedFilter)
        {
            matchedFilter = false;
            string name = Path.GetFileNameWithoutExtension(fileName);
            if (name.EndsWith(ModUtils.MOD_FILE_EX_DBPF, StringComparison.OrdinalIgnoreCase))
                name = name.Substring(0, name.Length - ModUtils.MOD_FILE_EX_DBPF.Length);

            Cmd.WriteLine($"{nameof(HazardMatchLoosePackage)}(");
            Cmd.WriteLine($"\t{nameof(fileName)}: {fileName},");
            Cmd.WriteLine($"\t{nameof(name)}: {name}");
            Cmd.WriteLine(")");
            /*bool anyMatched = HAZARDOUS_LOOSE_PACKAGE_NAMES.Any(x =>
            {
                Cmd.WriteLine($"{nameof(x)}: {x}");
                return x.Equals(name, StringComparison.OrdinalIgnoreCase);
            });
            if (!anyMatched)
                anyMatched = HAZARDOUS_LOOSE_PACKAGE_FILTERS.Any(x => LikeOperator.LikeString(name, x, Microsoft.VisualBasic.CompareMethod.Text));
            if (anyMatched)*/
            
            if (HAZARDOUS_LOOSE_PACKAGE_NAMES.Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return true;
            
            if (HAZARDOUS_LOOSE_PACKAGE_FILTERS.Any(x => LikeOperator.LikeString(name, x, Microsoft.VisualBasic.CompareMethod.Text)))
            {
                matchedFilter = true;
                return true;
            }

            return false;
        }

        static readonly IReadOnlyList<string> _VANILLA_FRIENDLY_BEFORE_IT_WAS_COOL = new List<string>()
        {
            "SPORE MOD - Enhanced Color Picker",
            "Adventure_ExtraSize-InfiniteComplexity"
        }.AsReadOnly();

        public static bool VanillaFriendlyMatch(string unique)
        {
            if (_VANILLA_FRIENDLY_BEFORE_IT_WAS_COOL.Any(x => unique.Contains(x, StringComparison.OrdinalIgnoreCase)))
                return true;
            
            return false;
        }
    }
}
