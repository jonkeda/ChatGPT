namespace Markdig.ChatGpt.Model
{
    /// <summary>Describes the appearance of a list item's bullet style.</summary>
    public enum TextMarkerStyleModel
    {
        /// <summary>No marker.</summary>
        None,
        /// <summary>A solid disc circle.</summary>
        Disc,
        /// <summary>A hollow disc circle.</summary>
        Circle,
        /// <summary>A hollow square shape.</summary>
        Square,
        /// <summary>A solid square box.</summary>
        Box,
        /// <summary>A lowercase Roman numeral starting with the numeral i. For example, i, ii, iii, and iv. The numeral is automatically incremented for each item added to the list.</summary>
        LowerRoman,
        /// <summary>An uppercase Roman numeral starting with the numeral I. For example, I, II, III, and IV. The numeric value is automatically incremented for each item added to the list.</summary>
        UpperRoman,
        /// <summary>A lowercase ASCII character starting with the letter a. For example, a, b, and c. The character value is automatically incremented for each item added to the list.</summary>
        LowerLatin,
        /// <summary>An uppercase ASCII character starting with the letter A. For example, A, B, and C. The character value is automatically incremented for each item added to the list.</summary>
        UpperLatin,
        /// <summary>A decimal starting with the number one. For example, 1, 2, and 3. The decimal value is automatically incremented for each item added to the list.</summary>
        Decimal,
    }
}