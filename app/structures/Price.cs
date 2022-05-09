
public class Price
{
    public int and_price;

    public int not_price;

    public Price(): this(0,0)
    {}

    public Price(int and_gates, int not_gates) 
    {
        this.and_price = and_gates;
        this.not_price = not_gates;
    }

    public static Price operator +(Price obj1, Price obj2)
    {
        return new Price(obj1.and_price + obj2.and_price, obj1.not_price + obj2.not_price);
    }

    public static Price operator -(Price obj1, Price obj2)
    {
        return new Price(obj1.and_price - obj2.and_price, obj1.not_price - obj2.not_price);
    }

    public static bool operator <=(Price obj1, Price obj2)
    {
        return ((obj1.and_price <= obj2.and_price) && (obj1.not_price <= obj2.not_price));
    }

    public static bool operator >=(Price obj1, Price obj2)
    {
        return ((obj1.and_price >= obj2.and_price) && (obj1.not_price >= obj2.not_price));
    }

}