using System;
using System.Collections.Generic;
using System.Globalization;
using Lib.JSON.Utilities;

namespace Lib.JSON.Linq
{
    internal class JPath
    {
        private readonly string _expression;
        private int _currentIndex;

        public JPath(string expression)
        {
            ValidationUtils.ArgumentNotNull(expression, "expression");
            this._expression = expression;
            this.Parts = new List<object>();

            this.ParseMain();
        }

        public List<object> Parts { get; private set; }

        internal JToken Evaluate(JToken root, bool errorWhenNoMatch)
        {
            JToken current = root;

            foreach (object part in this.Parts)
            {
                string propertyName = part as string;
                if (propertyName != null)
                {
                    JObject o = current as JObject;
                    if (o != null)
                    {
                        current = o[propertyName];

                        if (current == null && errorWhenNoMatch)
                        {
                            throw new Exception("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, propertyName));
                        }
                    }
                    else
                    {
                        if (errorWhenNoMatch)
                        {
                            throw new Exception("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, propertyName, current.GetType().Name));
                        }

                        return null;
                    }
                }
                else
                {
                    int index = (int)part;

                    JArray a = current as JArray;

                    if (a != null)
                    {
                        if (a.Count <= index)
                        {
                            if (errorWhenNoMatch)
                            {
                                throw new IndexOutOfRangeException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.InvariantCulture, index));
                            }
              
                            return null;
                        }

                        current = a[index];
                    }
                    else
                    {
                        if (errorWhenNoMatch)
                        {
                            throw new Exception("Index {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, index, current.GetType().Name));
                        }

                        return null;
                    }
                }
            }

            return current;
        }

        private void ParseMain()
        {
            int currentPartStartIndex = this._currentIndex;
            bool followingIndexer = false;

            while (this._currentIndex < this._expression.Length)
            {
                char currentChar = this._expression[this._currentIndex];

                switch (currentChar)
                {
                    case '[':
                    case '(':
                        if (this._currentIndex > currentPartStartIndex)
                        {
                            string member = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex);
                            this.Parts.Add(member);
                        }

                        this.ParseIndexer(currentChar);
                        currentPartStartIndex = this._currentIndex + 1;
                        followingIndexer = true;
                        break;
                    case ']':
                    case ')':
                        throw new Exception(string.Format("Unexpected character while parsing path: {0}", currentChar));
                    case '.':
                        if (this._currentIndex > currentPartStartIndex)
                        {
                            string member = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex);
                            this.Parts.Add(member);
                        }
                        currentPartStartIndex = this._currentIndex + 1;
                        followingIndexer = false;
                        break;
                    default:
                        if (followingIndexer)
                        {
                            throw new Exception(string.Format("Unexpected character following indexer: {0}", currentChar));
                        }
                        break;
                }

                this._currentIndex++;
            }

            if (this._currentIndex > currentPartStartIndex)
            {
                string member = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex);
                this.Parts.Add(member);
            }
        }

        private void ParseIndexer(char indexerOpenChar)
        {
            this._currentIndex++;

            char indexerCloseChar = (indexerOpenChar == '[') ? ']' : ')';
            int indexerStart = this._currentIndex;
            int indexerLength = 0;
            bool indexerClosed = false;

            while (this._currentIndex < this._expression.Length)
            {
                char currentCharacter = this._expression[this._currentIndex];
                if (char.IsDigit(currentCharacter))
                {
                    indexerLength++;
                }
                else if (currentCharacter == indexerCloseChar)
                {
                    indexerClosed = true;
                    break;
                }
                else
                {
                    throw new Exception(string.Format("Unexpected character while parsing path indexer: {0}", currentCharacter));
                }

                this._currentIndex++;
            }

            if (!indexerClosed)
            {
                throw new Exception(string.Format("Path ended with open indexer. Expected {0}", indexerCloseChar));
            }

            if (indexerLength == 0)
            {
                throw new Exception("Empty path indexer.");
            }

            string indexer = this._expression.Substring(indexerStart, indexerLength);
            this.Parts.Add(Convert.ToInt32(indexer, CultureInfo.InvariantCulture));
        }
    }
}