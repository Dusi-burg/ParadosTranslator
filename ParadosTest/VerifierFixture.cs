using ParadosLib;

namespace ParadosTest
{
    [TestFixture]
    public class VerifierFixture
    {
        [TestCase("Command Power Allocation: $VALUE|H2$","Assegnazione Potere di Comando: $VALUE|H2$")]
        [TestCase("§RCancel: $NAME|H$§!", "§RCancella: $NAME|H$§!")]
        [TestCase("Due to the war of controllers: $SIDE1|H$ vs $SIDE2|H$", "a causa della guerra tra i controllori: $SIDE1|H$ vs $SIDE2|H$")]
        [TestCase("Hold [835.GetName]", "Mantenere [835.GetName]")]
        //"Hold [835.GetName]" => "Mantenere [835.GetName]"
        public void TraductionIsCompatible_CasoTrue(string orig, string trad)
        {
            bool result = Helper.TraductionIsCompatible(orig, trad);

            Assert.That(result, Is.True);
        }

        [TestCase("Hold [835.GetName]", "Mantenere 835.GetName")]
        public void TraductionIsCompatible_CasoMissingSquare(string orig, string trad)
        {
            bool result = Helper.TraductionIsCompatible(orig, trad);

            Assert.That(result, Is.False);
        }

        [TestCase("Hold [835.GetName]", "Mantenere [835.GetNames]")]
        public void TraductionIsCompatible_CasoDifferentSquare(string orig, string trad)
        {
            bool result = Helper.TraductionIsCompatible(orig, trad);

            Assert.That(result, Is.False);
        }

        [TestCase("Command Power Allocation: $VALUE|H2$", "Assegnazione Potere di Comando: value")]
        public void TraductionIsCompatible_CasoMissingProperty(string orig, string trad)
        {
            bool result = Helper.TraductionIsCompatible(orig, trad);

            Assert.That(result, Is.False);
        }

        [TestCase("Command Power Allocation: $VALUE|H2$", "Assegnazione Potere di Comando: $VALUE|H$")]
        public void TraductionIsCompatible_CasoDifferentValueProperty(string orig, string trad)
        {
            bool result = Helper.TraductionIsCompatible(orig, trad);

            Assert.That(result, Is.False);
        }

        [TestCase("The [FROM.GetAdjective] parliament has [FIN.GetAdjective] continuation war", "The [FROM.GetAdjective] parliament has [FIN.GetAdjective] continuation war")]
        public void ApplyUpperCaseOnSquare_CasoUguale(string orig, string expected)
        {
            string result = Helper.ApplyUpperCaseOnSquare(orig, out bool differs);

            Assert.That(differs, Is.False);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("The [FROM.getAdjective] parliament has [FIN.GetAdjective] continuation war", "The [FROM.GetAdjective] parliament has [FIN.GetAdjective] continuation war")]
        public void ApplyUpperCaseOnSquare_CasoDiversoPerPrimaLetteraDopoDot(string orig, string expected)
        {
            string result = Helper.ApplyUpperCaseOnSquare(orig, out bool differs);

            Assert.That(differs, Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("The [From.GetAdjective] parliament has [FIN.GetAdjective] continuation war", "The [FROM.GetAdjective] parliament has [FIN.GetAdjective] continuation war")]
        public void ApplyUpperCaseOnSquare_CasoDiversoPerPropieta(string orig, string expected)
        {
            string result = Helper.ApplyUpperCaseOnSquare(orig, out bool differs);

            Assert.That(differs, Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("The [FROM.getAdjective] parliament has politics of [FROM.getName] be they internal or foreign.", "The [FROM.GetAdjective] parliament has politics of [FROM.GetName] be they internal or foreign.")]
        public void ApplyUpperCaseOnSquare_CasoDiversoLetteraMaDueErrori(string orig, string expected)
        {
            string result = Helper.ApplyUpperCaseOnSquare(orig, out bool differs);

            Assert.That(differs, Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("After the early rounds of our ongoing negotiations with [?FIN_faction_leader_recieving_resource_rights.GetNameDef] progressing", "After the early rounds of our ongoing negotiations with [?FIN_faction_leader_recieving_resource_rights.GetNameDef] progressing")]
        public void ApplyUpperCaseOnSquare_CasoProprietaConUnderscore(string orig, string expected)
        {
            string result = Helper.ApplyUpperCaseOnSquare(orig, out bool differs);

            Assert.That(differs, Is.False);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("After the early rounds of our ongoing negotiations with [?FIN_faction_leader_recieving_resource_rights.getNameDef] progressing", "After the early rounds of our ongoing negotiations with [?FIN_faction_leader_recieving_resource_rights.GetNameDef] progressing")]
        public void ApplyUpperCaseOnSquare_CasoProprietaConUnderscoreDiversoPerPrimaLetteraDopoDot(string orig, string expected)
        {
            string result = Helper.ApplyUpperCaseOnSquare(orig, out bool differs);

            Assert.That(differs, Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("[FROM.GetLeader] of [FROM.GetNameDef] has foolishly rejected our military cooperation proposal. It may be that the [FROM.GetAdjective]s simply don't want"
            , "[FROM.GetLeader] di [FROM.GetName] ha respinto la nostra proposta di cooperazione militare. Potrebbe essere che [FROM.GetAdjective] semplicemente non voglia"
            , "[FROM.GetLeader] di [FROM.GetNameDef] ha respinto la nostra proposta di cooperazione militare. Potrebbe essere che [FROM.GetAdjective] semplicemente non voglia")]
        public void ApplySquareAdjustament_CasoFROMGetNameDef(string orig, string traduced, string expected)
        {
            bool result = Helper.ApplySquareAdjustament(orig, traduced, out string adjusted);

            Assert.That(result, Is.True);
            Assert.That(adjusted, Is.EqualTo(expected));
        }

        [TestCase]
        public void ApplySquareAdjustament_CasoSpeciale()
        {
            string original = @"Following increasingly autocratic politics, [FRANCE.GetName] has announced that it is time they find allies to counter the threat of global communism. Now it seems that [FROM.GetName] is that ally.\n\nThis morning, [FROM.GetLeader] announced that [FRANCE.GetName] and [FROM.GetName] will stand together as brothers. 'Our shared heritage will no longer make us enemies, but allies! The Titans of Europe stand together.'";

            string traduced = @"Dopo scelte politiche sempre più autocratiche, [FRANCE.GetNameDef] ha annunciato che è tempo di trovare alleati per contrastare la minaccia del Comunismo internazionale. Allo stato attuale, sembrerebbe che [FROM.GetNameDef] sia l'alleato ideale.\n\nQuesta mattina, [FROM.GetLeader] ha annunciato che [FRANCE.GetNameDef] e [FROM.GetNameDef] faranno fronte comune come fratelli in armi. 'La nostra storia comune non sarà più causa di divisioni, ma di unione! I titani dell'Europa ora fanno fronte comune.'";
            string expected = @"Dopo scelte politiche sempre più autocratiche, [FRANCE.GetName] ha annunciato che è tempo di trovare alleati per contrastare la minaccia del Comunismo internazionale. Allo stato attuale, sembrerebbe che [FROM.GetName] sia l'alleato ideale.\n\nQuesta mattina, [FROM.GetLeader] ha annunciato che [FRANCE.GetName] e [FROM.GetName] faranno fronte comune come fratelli in armi. 'La nostra storia comune non sarà più causa di divisioni, ma di unione! I titani dell'Europa ora fanno fronte comune.'";

            bool result = Helper.ApplySquareAdjustament(original, traduced, out string adjusted);

            Assert.That(result, Is.True);
            Assert.That(adjusted, Is.EqualTo(expected));
        }
    }
}