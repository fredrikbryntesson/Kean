// 
//  Generator.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika 2012
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Math.Random
{
    public abstract class Generator<T> :
        IGenerator<T>
    {
        static int[] shifts;
        static ulong[] seeds;
        static int shiftCounter = 0;
        static int seedCounter = 0;
        int a, b, c;
        ulong x, y, z;
        ulong w;
        static Generator()
        {
            Generator<T>.shifts = new int[] 
            {
                1, 1,54, 1, 1,55, 1, 3,45, 1, 7, 9, 1, 7,44, 1, 7,46, 1, 9,50, 1,11,35, 1,11,50,
                1,13,45, 1,15, 4, 1,15,63, 1,19, 6, 1,19,16, 1,23,14, 1,23,29, 1,29,34, 1,35, 5,
                1,35,11, 1,35,34, 1,45,37, 1,51,13, 1,53, 3, 1,59,14, 2,13,23, 2,31,51, 2,31,53,
                2,43,27, 2,47,49, 3, 1,11, 3, 5,21, 3,13,59, 3,21,31, 3,25,20, 3,25,31, 3,25,56,
                3,29,40, 3,29,47, 3,29,49, 3,35,14, 3,37,17, 3,43, 4, 3,43, 6, 3,43,11, 3,51,16,
                3,53, 7, 3,61,17, 3,61,26, 4, 7,19, 4, 9,13, 4,15,51, 4,15,53, 4,29,45, 4,29,49,
                4,31,33, 4,35,15, 4,35,21, 4,37,11, 4,37,21, 4,41,19, 4,41,45, 4,43,21, 4,43,31,
                4,53, 7, 5, 9,23, 5,11,54, 5,15,27, 5,17,11, 5,23,36, 5,33,29, 5,41,20, 5,45,16,
                5,47,23, 5,53,20, 5,59,33, 5,59,35, 5,59,63, 6, 1,17, 6, 3,49, 6,17,47, 6,23,27,
                6,27, 7, 6,43,21, 6,49,29, 6,55,17, 7, 5,41, 7, 5,47, 7, 5,55, 7, 7,20, 7, 9,38,
                7,11,10, 7,11,35, 7,13,58, 7,19,17, 7,19,54, 7,23, 8, 7,25,58, 7,27,59, 7,33, 8,
                7,41,40, 7,43,28, 7,51,24, 7,57,12, 8, 5,59, 8, 9,25, 8,13,25, 8,13,61, 8,15,21,
                8,25,59, 8,29,19, 8,31,17, 8,37,21, 8,51,21, 9, 1,27, 9, 5,36, 9, 5,43, 9, 7,18,
                9,19,18, 9,21,11, 9,21,20, 9,21,40, 9,23,57, 9,27,10, 9,29,12, 9,29,37, 9,37,31,
                9,41,45,10, 7,33,10,27,59,10,53,13,11, 5,32,11, 5,34,11, 5,43,11, 5,45,11, 9,14,
                11, 9,34,11,13,40,11,15,37,11,23,42,11,23,56,11,25,48,11,27,26,11,29,14,11,31,18,
                11,53,23,12, 1,31,12, 3,13,12, 3,49,12, 7,13,12,11,47,12,25,27,12,39,49,12,43,19,
                13, 3,40,13, 3,53,13, 7,17,13, 9,15,13, 9,50,13,13,19,13,17,43,13,19,28,13,19,47,
                13,21,18,13,21,49,13,29,35,13,35,30,13,35,38,13,47,23,13,51,21,14,13,17,14,15,19,
                14,23,33,14,31,45,14,47,15,15, 1,19,15, 5,37,15,13,28,15,13,52,15,17,27,15,19,63,
                15,21,46,15,23,23,15,45,17,15,47,16,15,49,26,16, 5,17,16, 7,39,16,11,19,16,11,27,
                16,13,55,16,21,35,16,25,43,16,27,53,16,47,17,17,15,58,17,23,29,17,23,51,17,23,52,
                17,27,22,17,45,22,17,47,28,17,47,29,17,47,54,18, 1,25,18, 3,43,18,19,19,18,25,21,
                18,41,23,19, 7,36,19, 7,55,19,13,37,19,15,46,19,21,52,19,25,20,19,41,21,19,43,27,
                20, 1,31,20, 5,29,21, 1,27,21, 9,29,21,13,52,21,15,28,21,15,29,21,17,24,21,17,30,
                21,17,48,21,21,32,21,21,34,21,21,37,21,21,38,21,21,40,21,21,41,21,21,43,21,41,23,
                22, 3,39,23, 9,38,23, 9,48,23, 9,57,23,13,38,23,13,58,23,13,61,23,17,25,23,17,54,
                23,17,56,23,17,62,23,41,34,23,41,51,24, 9,35,24,11,29,24,25,25,24,31,35,25, 7,46,
                25, 7,49,25, 9,39,25,11,57,25,13,29,25,13,39,25,13,62,25,15,47,25,21,44,25,27,27,
                25,27,53,25,33,36,25,39,54,28, 9,55,28,11,53,29,27,37,31, 1,51,31,25,37,31,27,35,
                33,31,43,33,31,55,43,21,46,49,15,61,55, 9,56
            };
            Generator<T>.seeds = new ulong[] 
            {
                111643631875061	,	376742632075100	,	104312041819228	,
                212159179176831	,	586788705431687	,	932054368519732	,
                104384448266558	,	37649888290870	,	12462418101806	,
                367922623633266	,	665836088197395	,	20461367422852	,
                73944477312032	,	514148238438370	,	2498070972605	,
                495631287214063	,	931047848372717	,	687141804825005	,
                642084823741701	,	731173324242314	,	830492193392813	,
                514867287538605	,	293418074980151	,	338355260279147	,
                997562636064516	,	17971484789610	,	4638171419227	,
                713255521713015	,	7290743145117	,	33827928732953	,
                652859719812138	,	530109460115838	,	168056100443516	,
                172645732738224	,	293013138138109	,	349766569298566	,
                956399975431199	,	92558683684985	,	510923778040261	,
                615557640486210	,	118081284145147	,	974386002212645	,
                78137987684689	,	87130792258153	,	849676453979493	,
                624909921584987	,	287591917714733	,	245502806768894	,
                242166344421283	,	70449272937284	,	132113748510415	,
                169759542833226	,	559033160543817	,	22250391846999	,
                591383954271168	,	576099151077904	,	742445094031553	,
                294785965250414	,	319668791287154	,	129444986296566	,
                961559500927429	,	72918845778134	,	484072606158754	,
                296216658362966	,	124682024514015	,	4014357133980	,
                126397529171020	,	172654159864074	,	646296329353307	,
                145443713454714	,	24016322826831	,	193861545717999	,
                83403888279834	,	113336843131706	,	26652471134593	,
                67642520430697	,	448069698968403	,	856214555635434	,
                640419901114610	,	40273948262864	,	15738227481446	,
                170489452397444	,	599041693639384	,	97551962063932	,
                930398941652795	,	10631972868202	,	20963247755494	,
                822443439296607	,	172205198410753	,	762725098597593	,
                969905524470693	,	252554002923289	,	48819715960172	,
                91197480397303	,	887015138073143	,	982517863527556	,
                576664120447589	,	783643826694393	,	707135338879865	,
                464926159426729	,	582728175414648	,	772101292353712	,
                8433191728320	,	208391371510597	,	172343935574816	,
                100117500486054	,	411901006119660	,	35006841257812	,
                924206543116530	,	55471068388102	,	301768475010115	,
                35542558657304	,	470104323357022	,	521618297647981	,
                865952624718552	,	294913371232285	,	648446939541387	,
                721153498558036	,	99137474826204	,	241382427216196	,
                74285886396023	,	889365134370958	,	967687431727176	,
                353792792228906	,	550132693748174	,	41973607465315	,
                109822280710920	,	262992359364629	,	578011043743965	,
                901273334177806	,	124461544449244	,	472771134615884	,
                630021299023510	,	687744898320481	,	598156724817076	,
                407798638248454	,	652699123945989	,	453895484777919	,
                432161260818167	,	846319405882458	,	151397685686019	,
                961676437574108	,	93643920498855	,	590515649211933	,
                709756269335684	,	726072302637004	,	32989248431128	,
                858126187523570	,	757542909040264	,	803994725440135	,
            };
        }
        protected Generator()
            : this((ulong)DateTime.Now.Ticks, (int)((DateTime.Now.Ticks / 1000) % Generator<T>.shifts.Length) / 3, (int)((DateTime.Now.Ticks / 1000) % Generator<T>.seeds.Length) / 3)
        { }
        protected Generator(ulong seed, int shiftCounter, int seedCounter)
        {
            this.a = Generator<T>.shifts[3 * shiftCounter + 0];
            this.b = Generator<T>.shifts[3 * shiftCounter + 1];
            this.c = Generator<T>.shifts[3 * shiftCounter + 2];
            this.x = seed;
            this.y = Generator<T>.seeds[3 * seedCounter + 0];
            this.z = Generator<T>.seeds[3 * seedCounter + 1];
            this.w = Generator<T>.seeds[3 * seedCounter + 2];
        }
        protected ulong Next()
        {
            ulong t = this.x ^ (this.x << this.a);
            this.x = this.y;
            this.y = this.z;
            this.z = this.w;
            return this.w = (this.w ^ (this.w >> this.c)) ^ (t ^ (t >> this.b));
        }
        protected ulong Next(int bits)
        {
            return bits == 0 ? 0 : this.Next() >> (64 - bits);
        }
        public abstract T Generate();
        public virtual T[] Generate(int count)
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
                result[i] = this.Generate();
            return result;
        }
        public virtual T[] GenerateUnique(int count)
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
				do
					result[i] = this.Generate();
				while (result.Exists(item => item.Equals(result[i]), i));
            }
            return result;
        }
    }
}