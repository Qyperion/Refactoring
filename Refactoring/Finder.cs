using System;
using System.Collections.Generic;

namespace Refactoring
{
    public class Finder
    {
        private readonly List<Result> _p;

        public Finder(List<Result> p)
        {
            _p = p;
        }

        public P Find(PT pt)
        {
            var ps = new List<P>();

            for(var i = 0; i < _p.Count - 1; i++)
            {
                for(var j = i + 1; j < _p.Count; j++)
                {
                    var p = new P();
                    if(_p[i].DoB < _p[j].DoB)
                    {
                        p.P1 = _p[i];
                        p.P2 = _p[j];
                    }
                    else
                    {
                        p.P1 = _p[j];
                        p.P2 = _p[i];
                    }
                    p.D = p.P2.DoB - p.P1.DoB;
                    ps.Add(p);
                }
            }

            if(ps.Count < 1)
            {
                return new P();
            }

            var temp = ps[0];
            for(var i = 0; i < ps.Count; i++)
            {
                switch(pt)
                {
                    case PT.Minus:
                        if(ps[i].D < temp.D)
                        {
                            temp = ps[i];
                        }
                        break;

                    case PT.Plus:
                        if(ps[i].D > temp.D)
                        {
                            temp = ps[i];
                        }
                        break;
                }
            }

            return temp;
        }
    }
}