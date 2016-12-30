using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa_s_ezreal_tutorial
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    class Program
    {
        static void Main(string[] args)
        {
            // triggers once loading is completed
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            Game.OnTick += Game_OnTick;
        }

        // declare menu
        private static Menu EzrealMenu, ComboMenu;

        // class Player.Instance which is your player
        private static AIHeroClient User = Player.Instance;

        // declare Ezreal Q as skillshot
        private static Spell.Skillshot Q;

        // declare Ezreal W as skillshot
        private static Spell.Skillshot W;

        // declare Ezreal E as a skillshot
        private static Spell.Skillshot E;

        // declare Ezreal R as a skillshot
        private static Spell.Skillshot R;

        private static void Game_OnTick(EventArgs args)
        {
            // if combo mode is active
            // NOTE: Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo) will only trigger if the Combo mode is the only mode active.
            //       And Using Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) Will trigger even if other modes are active
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
        }
        
        // Cast Q, W, E, R if a target is in range
        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            // if a target is not in range of our q skip trying to combo target
            if (target == null)
            {
                return; 
            }

            // if checkbox for q is checked
            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                var Qpred = Q.GetPrediction(target);

                if (target.IsValidTarget(Q.Range) && Q.IsReady() && Qpred.HitChance >= HitChance.High)
                {
                    // Cast already applies the prediction so it's not needed to use Qpred.CastPosition
                    Q.Cast(target);
                }
            }

            if (ComboMenu["W"].Cast<CheckBox>().CurrentValue)
            {
                var Wpred = W.GetPrediction(target);

                if (target.IsValidTarget(W.Range) && W.IsReady() && Wpred.HitChance >= HitChance.High)
                {
                    // Cast already applies the prediction so it's not needed to use Qpred.CastPosition
                    W.Cast(target);
                }
            }

            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
            {
                var Epred = E.GetPrediction(target);

                if (target.IsValidTarget(E.Range) && Q.IsReady() && Epred.HitChance >= HitChance.High)
                {
                    // Cast already applies the prediction so it's not needed to use Qpred.CastPosition
                    E.Cast(target);
                }
            }

            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {
                var Rpred = R.GetPrediction(target);

                if (target.IsValidTarget(R.Range) && R.IsReady() && Rpred.HitChance >= HitChance.High)
                {
                    // Cast already applies the prediction so it's not needed to use Qpred.CastPosition
                    R.Cast(target);
                }
            }

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            // create main menu
            EzrealMenu = MainMenu.AddMenu("KappaEz", "KappaEz");

            // create submenu
            ComboMenu = EzrealMenu.AddSubMenu("Combo");

            // add check boxes to submenu
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("R", new CheckBox("Use R"));

            // We only want to load our addon for ezreal so we skip every other champion
            if (User.ChampionName != "Ezreal") { return; }

            Q = new Spell.Skillshot(spellSlot: SpellSlot.Q, spellRange: 1150, skillShotType: SkillShotType.Linear, castDelay: 250, spellSpeed: 2000, spellWidth: 60)
            { AllowedCollisionCount = 0 }; // can only hit 1 unit

            W = new Spell.Skillshot(spellSlot: SpellSlot.W, spellRange: 1000, skillShotType: SkillShotType.Linear, castDelay: 250, spellSpeed: 1550, spellWidth: 80)
            { AllowedCollisionCount = int.MaxValue }; // can hit unlimited nmber of units

            E = new Spell.Skillshot(spellSlot: SpellSlot.E, spellRange: 475, skillShotType: SkillShotType.Circular, castDelay: 250, spellSpeed: null, spellWidth: 750);

            R = new Spell.Skillshot(spellSlot: SpellSlot.R, spellRange: int.MaxValue, skillShotType: SkillShotType.Linear, castDelay: 1000, spellSpeed: 2000, spellWidth: 160)
            { AllowedCollisionCount = int.MaxValue };
        }
    }
}
