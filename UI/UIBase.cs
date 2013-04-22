﻿#region File Description
//-----------------------------------------------------------------------------
// UIBase
//
// Copyright © 2010 - 2013 Wave Coorporation. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.UI;
#endregion

namespace WaveEngine.Components.UI
{
    /// <summary>
    /// UI decorator base class
    /// </summary>
    public abstract class UIBase : BaseDecorator
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is border.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is border; otherwise, <c>false</c>.
        /// </value>
        public bool IsBorder
        {
            get
            {
                return this.entity.FindComponent<BorderRenderer>() != null ? true : false;
            }

            set
            {
                if (value)
                {
                    if (this.entity.FindComponent<BorderRenderer>() == null)
                    {
                        this.entity.AddComponent(new BorderRenderer());
                    }
                }
                else
                {
                    BorderRenderer borderRenderer = this.entity.FindComponent<BorderRenderer>();
                    if (borderRenderer != null)
                    {
                        this.entity.RemoveComponent<BorderRenderer>();
                    }
                }

                this.entity.RefreshDependencies();
            }
        }

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>
        /// The color of the border.
        /// </value>
        /// <exception cref="System.InvalidOperationException">There isn't Border component. You must set Border=true</exception>
        public Color BorderColor
        {
            get
            {
                BorderRenderer borderRenderer = this.entity.FindComponent<BorderRenderer>();
                if (borderRenderer != null)
                {
                    return borderRenderer.Color;
                }
                else
                {
                    throw new InvalidOperationException("There isn't Border component. You must set Border=true");
                }
            }

            set
            {
                BorderRenderer borderRenderer = this.entity.FindComponent<BorderRenderer>();
                if (borderRenderer != null)
                {
                    borderRenderer.Color = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the element draw order.
        /// </summary>
        /// <value>
        /// The element draw order [0-1].
        /// </value>
        public float DrawOrder
        {
            get
            {
                Transform2D transform = this.entity.FindComponent<Transform2D>();
                if (transform != null)
                {
                    return transform.DrawOrder;
                }
                else
                {
                    throw new InvalidOperationException("There isn't Transform2D component.");
                }
            }

            set
            {
                Transform2D transform = this.entity.FindComponent<Transform2D>();
                if (transform != null)
                {
                    transform.DrawOrder = value;
                }
            }
        }
        #endregion

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <param name="value">The value.</param>
        public void SetValue(DependencyProperty dp, object value)
        {
            Control control = this.entity.FindComponentOfType<Control>();
            if (control != null)
            {
                control.SetValue(dp, value);
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <returns>DependencyProperty value</returns>
        public object GetValue(DependencyProperty dp)
        {
            object result = null;

            Control control = this.entity.FindComponentOfType<Control>();
            if (control != null)
            {
                result = control.GetValue(dp);
            }

            return result;
        }
    }
}