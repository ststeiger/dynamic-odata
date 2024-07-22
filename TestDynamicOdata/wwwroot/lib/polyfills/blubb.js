
// "use strict";
console.log("what is this", this);

if (true)
{
    /*
    class NotImplemented extends Error 
    {
        constructor(message = "", data:any, ...args:any[]) 
        {
            super(message, ...args);
            this.message = message + " has not yet been implemented.";
        }
    }


    class Foobar
    {
        private m_name: string;


        get name(): string
        {
            return this.m_name;
        }
        set name(value: string)
        {
            this.m_name = value;
        }

        constructor(name: string)
        {
            this.m_name = name;
        }
    }
    */

    var Foobar = /** @class */ (function ()
    {
        function Foobar(name)
        {
            this.m_name = name;
        }
        Object.defineProperty(Foobar.prototype, "name", {
            get: function ()
            {
                return this.m_name;
            },
            set: function (value)
            {
                this.m_name = value;
            },
            enumerable: false,
            configurable: true
        });
        return Foobar;
    }());

}
