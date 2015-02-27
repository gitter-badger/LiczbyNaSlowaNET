﻿
// Copyright (c) 2014 Przemek Walkowski

using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiczbyNaSlowaNET
{
    internal class ConverterAlgorithm : IConverterBuldier
    {
        [Inject]
        public IDictionaries dictionaries { get; set; }

        private StringBuilder result = new StringBuilder();

        public int[] Numbers { get; set;}

        // liczba setek
        private int hundreds;

        //liczba dziesiątek
        private int tens;

        //liczba jedności
        private int unity;

        //liczba nastek (11,12,13 itd)
        private int othersTens;

        //rząd wielkości (tysiąc, milion, miliard)
        private int order;

        //forma gramatyczna (tysiąc, tysiące, tysięcy)
        private int grammarForm;

        private int[] tempGrammarForm = new int[] { 2, 3, 4 };

        public string Build()
        {
            foreach (var number in Numbers)
            {
                var partialResult = new StringBuilder();

                if (number == 0)
                {
                    partialResult.Append(dictionaries.Unity[10]).ToString();
                }

                if (number < 0)
                {
                    partialResult.Append(dictionaries.Sign[2]);
                }

                var tempNumber = number;

                this.order = 0;

                while (tempNumber != 0)
                {
                    this.hundreds = (tempNumber % 1000) / 100;

                    this.tens = (tempNumber % 100) / 10;

                    this.unity = tempNumber % 10;

                    if (this.tens == 1 && this.unity > 0)
                    {
                        this.othersTens = this.unity;
                        this.tens = 0;
                        this.unity = 0;
                    }
                    else
                    {
                        this.othersTens = 0;
                    }

                    if (this.unity == 1 && (this.hundreds + this.tens + this.othersTens == 0))
                    {
                        this.grammarForm = 0;
                    }
                    else if (tempGrammarForm.Contains(this.unity))
                    {
                        this.grammarForm = 1;
                    }
                    else
                    {
                        this.grammarForm = 2;
                    }

                    if ((this.hundreds + this.unity + this.othersTens + this.tens) > 0)
                    {
                        var temp = partialResult.ToString().Trim();

                        partialResult.Clear();

                        partialResult.AppendFormat("{0}{1}{2}{3}{4}{5}",
                            this.CheckWhitespace(dictionaries.Hundreds[this.hundreds]),
                            this.CheckWhitespace(dictionaries.Tens[this.tens]),
                            this.CheckWhitespace(dictionaries.OthersTens[this.othersTens]),
                            this.CheckWhitespace(dictionaries.Unity[this.unity]),
                            this.CheckWhitespace(dictionaries.Endings[this.order, this.grammarForm]),
                            this.CheckWhitespace(temp));
                    }

                    this.order += 1;

                    tempNumber = tempNumber / 1000;
                }

                result.Append(partialResult.ToString().Trim());
                result.Append(" ");

            }

            return result.ToString().Trim();
            
        }

        private string CheckWhitespace(string ciag)
        {
            return String.IsNullOrEmpty(ciag) ? string.Empty : " " + ciag;
        }

      
    }
}
